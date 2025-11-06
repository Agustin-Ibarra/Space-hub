using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Repository;
using Stripe.Checkout;

namespace SpaceHub.Application.Controllers;

public class PaymentController : Controller
{
  private readonly IItemRespotory _itemRespotory;

  public PaymentController(IItemRespotory itemRespotory)
  {
    _itemRespotory = itemRespotory;
  }

  [Authorize]
  [HttpPost]
  [Route("/api/payment")]
  public async Task<IActionResult> ApiCreateCheckoutSession([FromBody] ItemsListDto itemsList)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else{
      List<SessionLineItemOptions> lineItems = [];
      foreach (var item in itemsList.ItemsList)
      {
        var itemData = await _itemRespotory.GetItemDetail(item.IdItem);
        if (itemData != null)
        {
          lineItems.Add(new SessionLineItemOptions
          {
            PriceData = new SessionLineItemPriceDataOptions
            {
              UnitAmount = (long)itemData.ItemUnitPrice * 100,
              Currency = "usd",
              ProductData = new SessionLineItemPriceDataProductDataOptions
              {
                Name = itemData.ItemName
              }
            },
            Quantity = item.Quantity
          });
        }
      }
      var domain = "http://localhost:5179";
      var options = new SessionCreateOptions
      {
        Mode = "payment",
        SuccessUrl = domain + "/purchase",
        CancelUrl = domain + "/cart",
        LineItems = lineItems
      };

      var services = new SessionService();
      Session session = services.Create(options);

      return Ok(new {idSession = session.Id});
    }
  }
}