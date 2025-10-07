using Microsoft.AspNetCore.Mvc;

namespace SpaceHub.Application.Controllers;

public class AstronomicalObjectController : Controller
{

  [HttpGet]
  [Route("/astronomical_objects")]
  public IActionResult AstronomicalObject()
  {
    return View();
  }

  [HttpGet]
  [Route("/astronomical_objects/info")]
  public IActionResult AstronomicalObjectInfo()
  {
    return View();
  }
}