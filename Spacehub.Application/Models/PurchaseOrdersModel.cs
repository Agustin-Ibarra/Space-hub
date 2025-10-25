using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class PurchaseOrder
{
  [Key]
  public int id_purchase { get; set; }
  public int id_customer { get; set; }
  public DateTime purchase_date { get; set; }
  public required string seller { get; set; }
  [Column(TypeName = "decimal(8,2)")]
  public decimal total { get; set; }

  [ForeignKey("id_customer")]
  public User? UserFk { get; set; }
  public ICollection<PurchaseDetail>? PurchaseDetailReference { get; set; }
}