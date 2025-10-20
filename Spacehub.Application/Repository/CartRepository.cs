using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Repository;

public interface ICartRepository
{
  Task<List<CartItemsDto>> GetCartItems(int idUser);
  Task AddItemToCart(Cart cart);
  Task DeleteItemToCart(Cart cart);
}

public class CartRepository : ICartRepository
{
  private readonly AppDbContext _appDbContext;

  public CartRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  public async Task<List<CartItemsDto>> GetCartItems(int idItem)
  {
    return await _appDbContext.Carts
    .OrderBy(cart => cart.id_cart)
    .Include(cart => cart.ItemFk)
    .Where(cart => cart.id_user == idItem)
    .Select(cartItem => new CartItemsDto
    {
      ItemName = cartItem.ItemFk != null ? cartItem.ItemFk.item_name : "Articulo sin nombre",
      IdCart = cartItem.id_cart,
      IdItem = cartItem.id_item,
      Quantity = cartItem.quantity,
      UnitPrice = cartItem.ItemFk != null ? cartItem.ItemFk.unit_price : 0,
      ImagePath = cartItem.ItemFk != null ? cartItem.ItemFk.path_image : "image_path"
    })
    .ToListAsync();
  }

  public async Task AddItemToCart(Cart cart)
  {
    _appDbContext.Carts.Add(cart);
    await _appDbContext.SaveChangesAsync();
  }

  public async Task DeleteItemToCart(Cart cart)
  {
    _appDbContext.Carts.Remove(cart);
    await _appDbContext.SaveChangesAsync();
  }
}