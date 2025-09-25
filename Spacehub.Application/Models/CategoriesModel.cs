using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Models;

public class Category
{
  [Key]
  public int id_category { get; set; }
  public required string category { get; set; }
  public int is_item { get; set; }

  public ICollection<Post>? PostReference { get; set; }
}