using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class AstronomicalObject
{
  [Key]
  public int id_object { get; set; }
  public int id_category { get; set; }
  public required string title { get; set; }
  public required string text_content { get; set; }
  public required string path_image { get; set; }

  [ForeignKey("id_category")]
  public Category? CategoryFk { get; set; }
}