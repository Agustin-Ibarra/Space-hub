using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace SpaceHub.Application.Repository;

public interface IItemRespotory
{
  Task<List<ItemDto>> GetListItems(int offset);
}

public class ItemRepository : IItemRespotory
{
  private readonly AppDbContext _appDbContext;
  public ItemRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  public async Task<List<ItemDto>> GetListItems(int offset)
  {
    return await _appDbContext.Items
    .OrderBy(item => item.id_item)
    .Select(item => new ItemDto
    {
      ItemImage = item.path_image,
      ItemName = item.item_name,
      IdItem = item.id_item,
      ItemUnitPrice = item.unit_price
    })
    .Take(10)
    .Skip(offset)
    .ToListAsync();
  }
}