using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Dtos;

public class PurchaseDto
{
  public int IdPurchaseOrder { get; set; }
  public DateTime DatePurchase { get; set; }
  public required string ItemName { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal Total { get; set; }
}

public class PurchaseDetailDto
{
  public required string ItemName { get; set; }
  public int Quantity { get; set; }
  public decimal UnitPrice { get; set; }
  public decimal Subtotal { get; set; }
}

public class PurchaseDetailListDto
{
  public List<PurchaseDetailDto>? PurchaseDeatilList { get; set; }
}