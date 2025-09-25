using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class User
{
  [Key]
  public int id_user { get; set; }
  public int id_role { get; set; }
  public required string fullname { get; set; }
  public required string username { get; set; }

  [EmailAddress]
  public required string email { get; set; }
  public required string user_password { get; set; }

  [ForeignKey("id_role")]
  public Role? RoleFk { get; set; }
  public ICollection<Chat>? ChatReference { get; set; }
  public ICollection<Cart>? CartReference { get; set; }
}