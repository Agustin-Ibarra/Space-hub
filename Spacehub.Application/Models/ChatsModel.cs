using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class Chat
{
  [Key]
  public int id_chat { get; set; }
  public int id_user { get; set; }
  public DateTime created_at { get; set; }

  [ForeignKey("id_user")]
  public User? UserFk { get; set; }
  public ICollection<ChatMessage>? ChatMessageReference { get; set; }
}