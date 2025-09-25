using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class Item
{
  [Key]
  public int id_item { get; set; }
  public int id_category { get; set; }
  public required string item_name { get; set; }
  public required string item_description { get; set; }
  public required string path_image { get; set; }
  public double unit_price { get; set; }
  public int stock { get; set; }

  [ForeignKey("id_category")]
  public Category? CategoryFk { get; set; }
  public ICollection<PurchaseDetail>? PurchaseDetailReference { get; set; }
  public ICollection<Cart>? CartReference { get; set; }
}