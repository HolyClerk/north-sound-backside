using System.Net;

namespace NorthSound.Backend.Domain.Responses;

public class GenericResponse <T>
{
    public ResponseStatus Status { get; set; } 

    public T? Data { get; set; } = default!;

    public string? Details { get; set; } 

    public static GenericResponse<T> Failed(string details, ResponseStatus responseStatus = ResponseStatus.Failed)
    {
        return new GenericResponse<T>() 
        {
            Details = details,
            Status = responseStatus,
        };
    }
    
    public static GenericResponse<T> Success(T data, string? details = null)
    {
        return new GenericResponse<T>()
        {
            Data = data,
            Details = details,
            Status = ResponseStatus.Success,
        };
    }
}