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
      decimal purchaseTotal = 0;
      decimal purchaseSubtotal = 0;
      try
      {
        foreach (var item in items.Items) // en este bucle se obtiene el total y subtotal
        {
          var itemPrice = await _itemRepository.GetItemPrice(item.IdItem);
          if (itemPrice != null)
          {
            purchaseTotal += itemPrice.UnitPrice * item.Quantity; // los descuentos o recargos se agregan aqui
            purchaseSubtotal += itemPrice.UnitPrice * item.Quantity;
          }
          else
          {
            return BadRequest(new { error = $"No existe el articulo con id: {item.IdItem}" });
          }
        }
        var purchase = new PurchaseOrder
        {
          seller = "Space Hub",
          id_customer = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier)),
          purchase_date = DateTime.Now,
          total = purchaseTotal
        };
        var purchaseOrder = await _purchaseRepository.CreatePurchaseOrder(purchase);
        foreach (var item in items.Items) // en este bucle se crea el detalle de orden de compra
        {
          var itemPrice = await _itemRepository.GetItemPrice(item.IdItem); // obtener el precio unitario del articulo
          var purchaseDetail = new PurchaseDetail
          {
            id_item = item.IdItem,
            id_purchase_order = purchase.id_purchase,
            quantity = item.Quantity,
            unit_price = itemPrice != null ? itemPrice.UnitPrice : 0,
            subtotal = purchaseSubtotal
          };
          await _purchaseRepository.CreatePurchaseDetail(purchaseDetail);
        }
        return Created("/api/purchase", new { total = purchaseTotal });
      }
      catch (Exception)
      {
        return StatusCode(503, "Ocurrio un error en la base de datos");
      }
    }
  }

  [Authorize]
  [HttpGet]
  [Route("/api/purchase")]
  public async Task<IActionResult> GetPurchaseByUser()
  {
    int idUser = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var purchaseList = await _purchaseRepository.GetPurchaseOrder(idUser);
    if (purchaseList != null)
    {
      return Ok(purchaseList);
    }
    else
    {
      return NotFound(new {error = "El usuario no tiene ordenes de compras"});
    }
  }
}