using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.Entities;

public class User
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

    public List<Song> Songs { get; set; } = new();
}
