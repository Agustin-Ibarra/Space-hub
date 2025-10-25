using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class PurchaseDetail
{
  [Key]
  public int id_purchase_detail { get; set; }
  public int id_purchase_order { get; set; }
  public int id_item { get; set; }
  public int quantity { get; set; }
  [Column(TypeName = "decimal(8,2)")]
  public decimal unit_price { get; set; }
  [Column(TypeName = "decimal(8,2)")]
  public decimal subtotal { get; set; }

  [ForeignKey("id_purchase_order")]
  public PurchaseOrder? PurchaseOrderFk { get; set; }

  [ForeignKey("id_item")]
  public ICollection<Item>? ItemReference { get; set; }
}