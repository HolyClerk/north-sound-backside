using NorthSound.Backend.Services.Abstractions;
using System.Security.Cryptography;

namespace NorthSound.Backend.Infrastructure;

public class StorageGenerator : IStorageGenerator
{
    public string GetNewGeneratedPath()
    {
        string generatedName = RandomNumberGenerator
            .GetInt32(-2_147_483_640, 2_147_483_640)
            .ToString();

        return @$"Z:\Storage\{generatedName}";
    }
}
