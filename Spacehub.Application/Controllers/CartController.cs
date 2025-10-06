using Microsoft.AspNetCore.Mvc;

namespace SpaceHub.Application.Controllers;

public class CartController : Controller
{
  [HttpGet]
  [Route("/cart")]
  public IActionResult Cart()
  {
    return View();
  }
}