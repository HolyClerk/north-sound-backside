using System.Net;

namespace NorthSound.Backend.Domain.Responses;

public class GenericResponse <T>
{
    public ResponseStatus Status { get; set; } 
    public T Data { get; set; } = default!;
}