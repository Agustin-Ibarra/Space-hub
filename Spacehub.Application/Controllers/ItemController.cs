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
  [Route("/items/section")]
  public IActionResult ItemSection()
  {
    return View();
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
  [Route("/api/items/{offset}/{idCategory}")]
  public async Task<IActionResult> ApiItemsList(int offset, int idCategory)
  {
    var items = await _itemRepository.GetListItems(offset, idCategory);
    if (items.Count < 1)
    {
      return NotFound(new { error = $"no existe la categoria {idCategory}" });

    }
    else
    {
      return Ok(items);
    }
  }

  [HttpGet]
  [Route("/api/items/detail/{idItem}")]
  public async Task<IActionResult> ApiItemDetail(int idItem)
  {
    var item = await _itemRepository.GetItemDetail(idItem);
    if (item != null)
    {
      return Ok(item);
    }
    else
    {
      return NotFound(new { error = $"No se encontro el articulo con id: {idItem}" });
    }
  }

  [Authorize]
  [HttpPatch]
  [Route("/api/items")]
  public async Task<IActionResult> ApiReserveItems([FromBody] ItemsListDto itemsList)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      List<ItemReserveDto> ItemsRejected = []; // lista de articulos rechazados por stock insuficiente
      foreach (var item in itemsList.ItemsList)
      {
        bool result = await _itemRepository.UpdateStock(item.IdItem, item.Quantity);
        if (result != true)
        {
          // si la consulta retorna false no se pudo actualizar el stock al reservar el articulo
          ItemsRejected.Add(item); // se agrega la informacion del articulo que no pudo ser actualizado en la lista
        }
      }
      if (ItemsRejected.Count == 0)
      {
        // si la lista esta vacia indica que no ocurrieron errores al actualizar stock
        return Created();
      }
      else
      {
        // en caso contrario se recorre la lista para restaurar stock original
        foreach (var item in itemsList.ItemsList)
        {
          int iter = 0;
          if (item.IdItem != ItemsRejected[iter].IdItem)
          {
            // se restaura el stock solo a los articulos que no fueron rechazados
            await _itemRepository.RestoreStock(item.IdItem, item.Quantity);
          }
        }
        return BadRequest(new
        {
          error = "No hay stock suficiente para algunos articulos",
          itemsRejected = ItemsRejected
        });
      }
    }
  }

  [Authorize]
  [HttpPatch]
  [Route("/api/items/restore")]
  public async Task<IActionResult> ApiRestoreItem([FromBody] ItemsListDto itemsList)
  {
    if (!ModelState.IsValid)
    {
      var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
      return BadRequest(new { error = errors });
    }
    else
    {
      foreach (var items in itemsList.ItemsList)
      {
        await _itemRepository.RestoreStock(items.IdItem, items.Quantity);
      }
      return NoContent();
    }
  }
}