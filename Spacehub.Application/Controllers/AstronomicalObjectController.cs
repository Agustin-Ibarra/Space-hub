using Microsoft.AspNetCore.Mvc;
using Spacehub.Application.Repository;
using SpaceHub.Application.Data;

namespace SpaceHub.Application.Controllers;

public class AstronomicalObjectController : Controller
{
  private readonly IAstronomicalObjectRepository _IAstronomicalObjectRepository;

  public AstronomicalObjectController(IAstronomicalObjectRepository astronomicalObjectRepository)
  {
    _IAstronomicalObjectRepository = astronomicalObjectRepository;
  }

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

  [HttpGet]
  [Route("/astronomical_objects/api/{offset}")]
  public async Task<IActionResult> AstronomicalObjectApi(int offset)
  {
    try
    {
      var astronomicalObjects = await _IAstronomicalObjectRepository.GetAstronomicalObjectList(offset);
      return Ok(astronomicalObjects);
    }
    catch (Exception)
    {
      return StatusCode(503, new {error = "Ocurrio un error en la base de datos"});
    }
  }
}