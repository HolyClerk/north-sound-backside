namespace NorthSound.Backend.Domain.Entities;

public class User
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
}
