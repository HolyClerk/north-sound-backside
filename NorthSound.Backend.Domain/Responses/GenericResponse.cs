using System.Net;

namespace NorthSound.Backend.Domain.Responses;

public class GenericResponse <T>
{
    public ResponseStatus Status { get; set; } 

    public T? Data { get; set; } = default!;

    public string? Details { get; set; } 

    public GenericResponse<T> Failed(string details)
    {
        Details = details;
        Status = ResponseStatus.Failed;
        return this;
    }
    
    public GenericResponse<T> Success(T data, string? details = null)
    {
        Data = data;
        Details = details;
        Status = ResponseStatus.Success;
        return this;
    }
}