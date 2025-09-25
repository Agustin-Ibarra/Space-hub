using System.ComponentModel.DataAnnotations;

namespace SpaceHub.Application.Models;

public class Role
{
  [Key]
  public int id_role { get; set; }
  public required string type_role { get; set; }
  public int access_level { get; set; }
  public ICollection<User>? UserReference { get; set; }
}