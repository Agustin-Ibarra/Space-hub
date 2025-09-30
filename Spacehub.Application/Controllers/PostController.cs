using Microsoft.AspNetCore.Mvc;

namespace SpaceHub.Application.Controllers;

public class PostController : Controller
{
  [HttpGet]
  [Route("/posts")]
  public IActionResult Post()
  {
    return View();
  }
}