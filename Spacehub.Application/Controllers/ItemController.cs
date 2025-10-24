using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpaceHub.Application.Dtos;
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
  public IActionResult Item()
  {
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
  public async Task<IActionResult> ApiItemsList(int offset)
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

  [HttpGet]
  [Route("/api/items/detail/{idItem}")]
  public async Task<IActionResult> ApiItemDetail(int idItem)
  {
    try
    {
      var item = await _itemRepository.GetItemDetail(idItem);
      return Ok(item);
    }
    catch (Exception)
    {
      return StatusCode(503, new { error = "Ocurrio un erro en la base de datos" });
    }
  }

  [Authorize]
  [HttpPatch]
  [Route("/api/items")]
  public async Task<IActionResult> ReserveItems([FromBody] ItemsListDto itemsList)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      List<ItemReserveDto> ItemsRejected = [];
      try
      {
        foreach (var item in itemsList.Items)
        {
          bool result = await _itemRepository.UpdateStock(item.IdItem, item.Quantity);
          if(result != true)
          {
            ItemsRejected.Add(item);
          }
        }
        if(ItemsRejected.Count == 0){
          return Ok(new {message = "Articulos reservados"});
        }
        else{
          foreach (var item in itemsList.Items)
          {
            int iter = 0;
            if(item.IdItem != ItemsRejected[iter].IdItem)
            {
              await _itemRepository.RestoreStock(item.IdItem, item.Quantity);
            }
          }
          return BadRequest(new
          {
            errorMessage = "No hay stock suficiente para algunos articulos",
            itemsRejected = ItemsRejected
          });
        }
      }
      catch (Exception ex)
      {
        return StatusCode(503,ex.Message);
      }
    }
  }
}