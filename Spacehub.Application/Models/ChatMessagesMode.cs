using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpaceHub.Application.Models;

public class ChatMessage
{
  [Key]
  public int id_message { get; set; }
  public int id_caht { get; set; }
  public required string user_message { get; set; }
  public required string ia_message { get; set; }
  public DateTime date_chat { get; set; }
  [ForeignKey("id_chat")]
  public ICollection<Chat>? ChatReference { get; set; }
}