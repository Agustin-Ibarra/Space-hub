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