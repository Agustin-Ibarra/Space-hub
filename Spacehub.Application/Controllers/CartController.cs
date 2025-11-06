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
  public CartController(ICartRepository cartRepository)
  {
    _cartRepository = cartRepository;
  }

  [Authorize]
  [HttpGet]
  [Route("/cart")]
  public IActionResult Cart()
  {
    return View();
  }

  [Authorize]
  [HttpPost]
  [Route("/api/cart")]
  public async Task<IActionResult> ApiAddItemToCart([FromBody] CartItemDto item)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      try
      {
        var cart = new Cart
        {
          id_item = item.IdItem,
          quantity = item.Quantity,
          id_user = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier))
        };
        await _cartRepository.AddItemToCart(cart);

        return Created("/cart", new { message = "Articulo agregado al carrito" });
      }
      catch (Exception)
      {
        return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
      }
    }
  }

  [Authorize]
  [HttpGet]
  [Route("/api/cart")]
  public async Task<IActionResult> ApiCartItems()
  {
    try
    {
      int idUser = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier));
      var CartiItems = await _cartRepository.GetCartItems(idUser);
      if (CartiItems.Count > 0)
      {
        return Ok(CartiItems);
      }
      else
      {
        return NotFound(new { errorMessage = "No se encontro resultados para este usuario" });
      }
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }

  [Authorize]
  [HttpDelete]
  [Route("/api/cart")]
  public async Task<IActionResult> ApiDeleteCart([FromBody] CartDto cartDto)
  {
    var cart = new Cart
    {
      id_user = Convert.ToInt16(User.FindFirstValue(ClaimTypes.NameIdentifier)),
      id_cart = cartDto.IdCart,
      id_item = cartDto.IdCart,
      quantity = cartDto.Quantity
    };
    try
    {
      await _cartRepository.DeleteItemToCart(cart);
      return NoContent();
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }
}