using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.Entities;

public class DialogueDTO
{
    [Key] public int Id { get; set; } = default!;

    [Required] public UserDTO FirstUser { get; set; } = default!;

    [Required] public UserDTO SecondUser { get; set; } = default!;

    [Required] public DateTime CreatedAt { get; set; } = default!;
}
