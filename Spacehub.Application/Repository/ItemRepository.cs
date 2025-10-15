using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace SpaceHub.Application.Repository;

public interface IItemRespotory
{
  Task<List<ItemDto>> GetListItems(int offset);
  Task<ItemDetailDto?> GetItemDetail(int idItem);
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

  public async Task<ItemDetailDto?> GetItemDetail(int idItem)
  {
    return await _appDbContext.Items
    .OrderBy(item => item.id_item)
    .Include(item => item.CategoryFk)
    .Where(Item => Item.id_item == idItem)
    .Select(item => new ItemDetailDto
    {
      ItemDescription = item.item_description,
      ItemImage = item.path_image,
      ItemName = item.item_name,
      IdItem = item.id_item,
      Itemstock = item.stock,
      ItemUnitPrice = item.unit_price,
      ItemCategory = item.CategoryFk != null ? item.CategoryFk.category : "Sin categoria"
    })
    .FirstOrDefaultAsync();
  }
}