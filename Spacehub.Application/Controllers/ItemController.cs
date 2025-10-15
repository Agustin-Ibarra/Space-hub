using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Repository;

namespace SpaceHub.Application.Controllers;

public class ItemController : Controller
{
  private readonly IItemRespotory _itemRepository;
  public ItemController(IItemRespotory itemRespotory)
  {
    _itemRepository = itemRespotory;
  }
  
  [HttpGet]
  [Route("/items")]
  public IActionResult Item() {
    return View();
  }

  [HttpGet]
  [Route("/items/details")]
  public IActionResult ItemDetail()
  {
    return View();
  }

  [HttpGet]
  [Route("/api/items/{offset}")]
  public async Task<IActionResult> GetItems(int offset)
  {
    try
    {
      var items = await _itemRepository.GetListItems(offset);
      return Ok(items);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un error en la base de datos" });
    }
  }
}