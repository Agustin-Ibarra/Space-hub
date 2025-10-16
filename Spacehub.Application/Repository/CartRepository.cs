using SpaceHub.Application.Data;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Repository;

public interface ICartRepository
{
  Task AddItemToCart(Cart cart);
}

public class CartRepository : ICartRepository
{
  private readonly AppDbContext _appDbContext;
  public CartRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }
  public async Task AddItemToCart(Cart cart)
  {
    _appDbContext.Carts.Add(cart);
    await _appDbContext.SaveChangesAsync();
  }
}