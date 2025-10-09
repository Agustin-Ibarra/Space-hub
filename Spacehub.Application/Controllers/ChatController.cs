using Microsoft.AspNetCore.Mvc;

namespace SpaceHub.Application.Controllers;

public class ChatController : Controller
{
  [HttpGet]
  [Route("/chat")]
  public IActionResult Chat()
  {
    return View();
  }
}