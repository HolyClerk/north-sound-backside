using System.Net;

namespace NorthSound.Backend.Domain.Responses;

public class BaseResponse <T>
{
    public ResponseStatus Status { get; set; } 
    public T ResponseData { get; set; } = default!;
}

public enum ResponseStatus
{
    Success,
    NotFound,
    BadRequest
}