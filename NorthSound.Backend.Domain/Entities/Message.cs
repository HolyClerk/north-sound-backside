using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Backend.Domain.Entities;

[Table(name: "Messages")]
public class Message
{
    [Key] public int Id { get; set; }

    [Required] public int DialogueId { get; set; }

    [Required] public int SenderId { get; set; }

    [Required] public int ReceiverId { get; set; }
  
    [Required] public string Value { get; set; } = default!;

    [Required] public DateTime CreatedAt { get; set; } = default!;

    public Dialogue Dialogue { get; set; } = default!;
    public User Sender { get; set; } = default!;
    public User Receiver { get; set; } = default!;
}
