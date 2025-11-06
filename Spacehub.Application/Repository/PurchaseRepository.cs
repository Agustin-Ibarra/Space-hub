using Microsoft.EntityFrameworkCore;
using SpaceHub.Application.Data;
using SpaceHub.Application.Dtos;
using SpaceHub.Application.Models;

namespace SpaceHub.Application.Repository;

public interface IPurchaseRepository
{
  Task<PurchaseOrder> CreatePurchaseOrder(PurchaseOrder purchaseOrder);
  Task CreatePurchaseDetail(PurchaseDetail purchaseDetail);
  Task<List<PurchaseDto>?> GetPurchaseOrder(int idUser);
  Task<List<PurchaseDetailDto>> GetPurchaseDetail(int idPurchaseOrder);
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

  public async Task<List<PurchaseDto>?> GetPurchaseOrder(int idPurchase)
  {
    return await _appDbContext.PurchaseDetails
    .OrderBy(purchase => purchase.id_purchase_detail)
    .Include(purchase => purchase.PurchaseOrderFk)
    .Include(purchase => purchase.ItemReferenceFk)
    .Where(purchase => purchase.id_purchase_order == idPurchase)
    .Select(purchase => new PurchaseDto
    {
      DatePurchase = purchase.PurchaseOrderFk != null ? purchase.PurchaseOrderFk.purchase_date : DateTime.Now,
      IdPurchaseOrder = purchase.id_purchase_order,
      ItemName = purchase.ItemReferenceFk != null ? purchase.ItemReferenceFk.item_name : "Articulo sin nombre",
      Quantity = purchase.quantity,
      Total = purchase.PurchaseOrderFk != null ? purchase.PurchaseOrderFk.total : 0,
      UnitPrice = purchase.ItemReferenceFk != null ? purchase.ItemReferenceFk.unit_price : 0
    })
    .ToListAsync();
  }

  public async Task<List<PurchaseDetailDto>> GetPurchaseDetail(int idPurchaseOrder)
  {
    return await _appDbContext.PurchaseDetails
    .OrderBy(purchase => purchase.id_purchase_detail)
    .Where(purchase => purchase.id_purchase_order == idPurchaseOrder)
    .Include(purchase => purchase.ItemReferenceFk)
    .Include(purchase => purchase.PurchaseOrderFk)
    .Select(purchase => new PurchaseDetailDto
    {
      ItemName = purchase.ItemReferenceFk != null ? purchase.ItemReferenceFk.item_name : "articulo sin nombre",
      Quantity = purchase.quantity,
      Subtotal = purchase.subtotal,
      UnitPrice = purchase.unit_price,
    })
    .ToListAsync();
  }
}