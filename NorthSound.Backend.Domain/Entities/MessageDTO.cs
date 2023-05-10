using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Backend.Domain.Entities;

[Table(name: "Messages")]
public class MessageDTO
{
    [Key] public int Id { get; set; } = default!;

    [Required] public DialogueDTO Dialogue { get; set; } = default!;

    [Required] public UserDTO Sender { get; set; } = default!;

    [Required] public UserDTO Receiver { get; set; } = default!;

    [Required] public string Message { get; set; } = default!;

    [Required] public DateTime CreatedAt { get; set; } = default!;
}
