using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;

namespace SpaceHub.Application.Repository;

public interface IItemRespotory
{
  Task<List<ItemDto>> GetListItems(int offset, int idCategory);
  Task<ItemDetailDto?> GetItemDetail(int idItem);
  Task<bool> UpdateStock(int idItem, int quantity);
  Task RestoreStock(int idItem, int quantity);
  Task<ItemUnitPriceDto?> GetItemPrice(int idItem);
}

public class ItemRepository : IItemRespotory
{
  private readonly AppDbContext _appDbContext;
  public ItemRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  public async Task<List<ItemDto>> GetListItems(int offset, int idCategory)
  {
    return await _appDbContext.Items
    .OrderBy(item => item.id_item)
    .Where(items => items.id_category == idCategory)
    .Select(item => new ItemDto
    {
      ItemImage = item.path_image,
      ItemName = item.item_name,
      IdItem = item.id_item,
      ItemUnitPrice = item.unit_price
    })
    .Skip(offset)
    .Take(12)
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

  public async Task<bool> UpdateStock(int idItem, int quantity)
  {
    var item = await _appDbContext.Items.FindAsync(idItem);
    if (item != null)
    {
      var result = await _appDbContext.Database.ExecuteSqlRawAsync(
        "update items set stock = stock - {1} where id_item = {0} and stock >= {1}",
        idItem, quantity
      );
      if (result == 1)
      {
        return true;
      }
      else
      {
        return false;
      }
    }
    else
    {
      return false;
    }
  }

  public async Task RestoreStock(int idItem, int quantity)
  {
    await _appDbContext.Database.ExecuteSqlRawAsync(
      "update items set stock = stock + {1} where id_item = {0}",
      idItem, quantity
    );
  }

  public async Task<ItemUnitPriceDto?> GetItemPrice(int idItem)
  {
    var itemPrice = await _appDbContext.Items
    .OrderBy(item => item.id_item)
    .Where(item => item.id_item == idItem)
    .Select(item => new ItemUnitPriceDto { UnitPrice = item.unit_price })
    .FirstOrDefaultAsync();
    return itemPrice;
  }

}