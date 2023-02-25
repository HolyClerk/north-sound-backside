using System.ComponentModel.DataAnnotations;

namespace NorthSound.Backend.Domain.Entities;

public class Song
{
    [Key] public int Id { get; set; } = default;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Author { get; set; } = default!;

    [Required]
    public Uri Path { get; set; } = default!;
}
