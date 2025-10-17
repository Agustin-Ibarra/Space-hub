using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class Cart
{
  [Key]
  public int id_cart { get; set; }
  public int id_user { get; set; }
  public int id_item { get; set; }
  public int quantity { get; set; }

  [ForeignKey("id_user")]
  public User? UserFk { get; set; }
  [ForeignKey("id_item")]
  public Item? ItemFk { get; set; }
}