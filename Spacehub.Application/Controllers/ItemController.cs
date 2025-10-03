using Microsoft.AspNetCore.Mvc;

namespace SpaceHub.Application.Controllers;

public class ItemController : Controller
{
  [HttpGet]
  [Route("/items")]
  public IActionResult Item(){
    return View();
  }
}