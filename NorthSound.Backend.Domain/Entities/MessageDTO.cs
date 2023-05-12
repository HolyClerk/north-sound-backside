using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Backend.Domain.Entities;

[Table(name: "Messages")]
public class MessageDTO
{
    [Key] public int Id { get; set; }

    [Required] public int DialogueId { get; set; }

    [Required] public int SenderId { get; set; }

    [Required] public int ReceiverId { get; set; }
  
    [Required] public string Message { get; set; } = default!;

    [Required] public DateTime CreatedAt { get; set; } = default!;

    public DialogueDTO Dialogue { get; set; } = default!;
    public UserDTO Sender { get; set; } = default!;
    public UserDTO Receiver { get; set; } = default!;
}
