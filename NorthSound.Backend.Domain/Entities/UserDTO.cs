using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthSound.Backend.Domain.SongEntities;

namespace NorthSound.Backend.Domain.Entities;

[Table(name: "Users")]
public class UserDTO
{
    [Key] public int Id { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string Name { get; set; } = default!;

    [Required]
    [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$")]
    public string Email { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = default!;

    [Required] public DateTime CreatedAt { get; set; } = default!;

    public List<SongDTO> Songs { get; set; } = new();
}
