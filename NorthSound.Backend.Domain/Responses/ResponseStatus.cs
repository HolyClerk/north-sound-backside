using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthSound.Backend.Domain.Responses;

public enum ResponseStatus
{
    Success,
    NotFound,
    BadRequest,
    Failed
}