using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NorthSound.Backend.Domain.Entities;

[Table(name: "Songs")]
public class Song
{
    [Key]
    public int Id { get; set; } = default;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(100, MinimumLength = 1)]
    public string Author { get; set; } = default!;

    [Required]
    public Uri Path { get; set; } = default!;

    public User? Owner { get; set; }
}
