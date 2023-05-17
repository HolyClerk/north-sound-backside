using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Backend.Domain.Entities;

[Table(name: "Dialogues")]
public class Dialogue
{
    [Key] public int Id { get; set; } = default!;

    [Required] public int FirstUserId { get; set; }

    [Required] public int SecondUserId { get; set; }

    [Required] public DateTime CreatedAt { get; set; } = default!;

    public User FirstUser { get; set; } = default!;
    public User SecondUser { get; set; } = default!;
    public List<Message> Messages { get; set; } = new();
}
