using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class CartController : Controller
{
  private readonly ICartRepository _cartRepository;
  public CartController (ICartRepository cartRepository)
  {
    _cartRepository = cartRepository;
  }
  [HttpGet]
  [Route("/cart")]
  public IActionResult Cart()
  {
    return View();
  }

  [HttpPost]
  [Route("/api/cart")]
  [Authorize]
  public async Task<IActionResult> AddItemToCart([FromBody] ItemCartDto item)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else{
      try
      {
        var cart = new Cart
        {
          id_item = item.IdItem,
          quantity = item.Quantity,
          id_user = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier))
        };
        await _cartRepository.AddItemToCart(cart);

        return Created("/cart",new {message = "Articulo agregado al carrito"});
      }
      catch (Exception)
      {
        return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
      }
    }
  }
}