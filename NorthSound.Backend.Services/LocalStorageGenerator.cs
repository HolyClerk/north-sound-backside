using NorthSound.Backend.Services.Abstractions;
using System.Security.Cryptography;

namespace NorthSound.Backend.Services;

public class LocalStorageGenerator : IStorageGenerator
{
    public string GenerateStoragePath()
    {
        string generatedName = RandomNumberGenerator
            .GetInt32(-2_147_483_640, 2_147_483_640)
            .ToString();

        return @$"Z:\Storage\{generatedName}";
    }
}
