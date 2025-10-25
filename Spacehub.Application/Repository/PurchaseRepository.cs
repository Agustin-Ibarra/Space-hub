using SpaceHub.Application.Data;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Repository;

public interface IPurchaseRepository
{
  Task<PurchaseOrder> CreatePurchaseOrder(PurchaseOrder purchaseOrder);
  Task CreatePurchaseDetail(PurchaseDetail purchaseDetail);
}

public class PurchaseRepository : IPurchaseRepository
{
  private readonly AppDbContext _appDbContext;

  public PurchaseRepository(AppDbContext appDbContext)
  {
    _appDbContext = appDbContext;
  }

  public async Task<PurchaseOrder> CreatePurchaseOrder(PurchaseOrder purchaseOrder)
  {
    _appDbContext.PurchaseOrders.Add(purchaseOrder);
    await _appDbContext.SaveChangesAsync();
    return purchaseOrder;
  }

  public async Task CreatePurchaseDetail(PurchaseDetail purchaseDetail)
  {
    _appDbContext.PurchaseDetails.Add(purchaseDetail);
    await _appDbContext.SaveChangesAsync();
  }
}