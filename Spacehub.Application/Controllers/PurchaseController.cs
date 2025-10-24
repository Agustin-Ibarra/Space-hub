using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class PurchaseControllers : Controller
{
  private readonly IItemRespotory _itemRepository;
  private readonly IPurchaseRepository _purchaseRepository;
  public PurchaseControllers(IItemRespotory itemRespotory, IPurchaseRepository purchaseRepository)
  {
    _itemRepository = itemRespotory;
    _purchaseRepository = purchaseRepository;
  }

  [Authorize]
  [HttpPost]
  [Route("/api/purchase")]
  public async Task<IActionResult> CreatePurchase([FromBody] ItemsListDto items)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      decimal total = 0;
      try
      {
        // primer paso se calcula el total de la lista de articulos
        foreach (var item in items.Items)
        {
          var itemPrice = await _itemRepository.GetItemPrice(item.IdItem);
          if (itemPrice != null)
          {
            total += itemPrice.UnitPrice * item.Quantity;
          }
          else
          {
            // en el caso de que el articulo es rechazado no continua al siguiente paso
            return BadRequest(new { error = $"No existe el articulo con id: {item.IdItem}" });
          }
        }
        // segundo paso crear las ordenes de compra
        var purchase = new PurchaseOrder
        {
          seller = "Space Hub",
          id_customer = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier)),
          purchase_date = DateTime.Now,
          total = total
        };
        var purchaseOrder = await _purchaseRepository.CreatePurchaseOrder(purchase);
        return Ok(new {idPurchase = purchase.id_purchase});
      }
      catch (Exception)
      {
        return StatusCode(503, "Ocurrio un error en la base de datos");
      }
    }
  }
}