using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class PurchaseController : Controller
{
  private readonly IItemRespotory _itemRepository;
  private readonly IPurchaseRepository _purchaseRepository;
  public PurchaseController(IItemRespotory itemRespotory, IPurchaseRepository purchaseRepository)
  {
    _itemRepository = itemRespotory;
    _purchaseRepository = purchaseRepository;
  }

  [Authorize]
  [HttpGet]
  [Route("/purchase")]
  public IActionResult Purchase()
  {
    return View();
  }

  [Authorize]
  [HttpPost]
  [Route("/api/purchase")]
  public async Task<IActionResult> ApiCreatePurchase([FromBody] ItemsListDto items)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      decimal purchaseOrderTotal = 0;
      foreach (var item in items.ItemsList) // en este bucle se obtiene el total y subtotal
      {
        var itemPrice = await _itemRepository.GetItemPrice(item.IdItem); // obtener el precio de cada articulo
        if (itemPrice != null)
        {
          purchaseOrderTotal += itemPrice.UnitPrice * item.Quantity; // los descuentos o recargos se agregan aqui
        }
        else
        {
          return BadRequest(new { error = $"No existe el articulo con id: {item.IdItem}" });
        }
      }

      var purchaseOrder = new PurchaseOrder // asignar valores al modelo de orden de compra
      {
        seller = "Space Hub",
        id_customer = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier)),
        purchase_date = DateTime.Now,
        total = purchaseOrderTotal
      };

      var purchaseOrdeData = await _purchaseRepository.CreatePurchaseOrder(purchaseOrder); // crear registro en DB

      foreach (var item in items.ItemsList) // en este bucle se crea el detalle de orden de compra por articulo
      {
        decimal purchaseSubtotal = 0;
        var itemPrice = await _itemRepository.GetItemPrice(item.IdItem); // obtener el precio unitario del articulo
        if (itemPrice != null)
        {
          purchaseSubtotal += itemPrice.UnitPrice * item.Quantity; // calcular el subtotal del detalle de orden de compra
          var purchaseDetail = new PurchaseDetail // asignar valores al modelo de detalle de orden de compra
          {
            id_item = item.IdItem,
            id_purchase_order = purchaseOrdeData.id_purchase,
            quantity = item.Quantity,
            unit_price = itemPrice != null ? itemPrice.UnitPrice : 0,
            subtotal = purchaseSubtotal
          };
          await _purchaseRepository.CreatePurchaseDetail(purchaseDetail); // crear registro en DB
        }
      }

      return Created("/api/purchase", new
      {
        idPurchase = purchaseOrdeData.id_purchase,
        datePurchase = purchaseOrdeData.purchase_date,
        totalPurchase = purchaseOrdeData.total
      });
    }
  }

  [Authorize]
  [HttpGet]
  [Route("/api/purchase/detail/{idPurchaseOrder}")]
  public async Task<IActionResult> ApiGetPurchaseDetails(int idPurchaseOrder)
  {
    var fullname = User.FindFirstValue(ClaimTypes.Name);
    var purchaseDetail = await _purchaseRepository.GetPurchaseDetail(idPurchaseOrder);
    if (purchaseDetail.Count > 0)
    {
      return Ok(new { purchases = purchaseDetail, userData = fullname });
    }
    else
    {
      return BadRequest(new { error = $"No existe orden de compra con id: {idPurchaseOrder}" });
    }
  }

  [Authorize]
  [HttpGet]
  [Route("/api/purchase")]
  public async Task<IActionResult> ApiGetPurchaseByUser()
  {
    int idUser = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
    var purchaseList = await _purchaseRepository.GetPurchaseOrder(idUser);
    if (purchaseList != null)
    {
      return Ok(purchaseList);
    }
    else
    {
      return NotFound(new { error = "El usuario no tiene ordenes de compras" });
    }
  }
}