using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Backend.Domain.Entities;

public class MessageDTO
{
    [Key] public int Id { get; set; } = default!;

    [ForeignKey(nameof(Dialogue))] public DialogueDTO Dialogue { get; set; } = default!;

    [ForeignKey(nameof(Sender))] public UserDTO Sender { get; set; } = default!;

    [ForeignKey(nameof(Receiver))] public UserDTO Receiver { get; set; } = default!;

    [Required] public string Message { get; set; } = default!;

    [Required] public DateTime CreatedAt { get; set; } = default!;
}
