using Microsoft.AspNetCore.Mvc;
using Spacehub.Application.Repository;

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
  [Route("/api/astronomical_objects/{offset}")]
  public async Task<IActionResult> ApiAstronomicalObject(int offset)
  {
    try
    {
      var astronomicalObjects = await _IAstronomicalObjectRepository.GetAstronomicalObjectList(offset);
      return Ok(astronomicalObjects);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos"});
    }
  }

  [HttpGet]
  [Route("/api/astronomical_objects/info/{idObject}")]
  public async Task<IActionResult> ApiAstronomicalObjectDeatil(int idObject)
  {
    try
    {
      var astronomicalObject = await _IAstronomicalObjectRepository.GetAstronomicalObject(idObject);
      return Ok(astronomicalObject);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos"});
    }
  }

  [HttpGet]
  [Route("/api/astronomical_objects/suggestion/{id}")]
  public async Task<IActionResult> ApiAstronomicalObjectsSuggestion(int id)
  {
    try
    {
      var astronomicalObjest = await _IAstronomicalObjectRepository.GetAstronomicalObjectSuggestion(id);
      return Ok(astronomicalObjest);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }
}