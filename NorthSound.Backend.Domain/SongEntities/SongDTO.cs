using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NorthSound.Backend.Domain.Entities;

namespace NorthSound.Backend.Domain.SongEntities;

[Table(name: "Songs")]
public class SongDTO
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

    public UserDTO? Owner { get; set; }
}
