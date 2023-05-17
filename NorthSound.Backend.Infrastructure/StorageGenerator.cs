using NorthSound.Backend.Services.Other;
using System.Security.Cryptography;

namespace NorthSound.Backend.Infrastructure;

public class StorageGenerator : IStorageGenerator
{
    public string GetNewGeneratedPath()
    {
        string generatedName = RandomNumberGenerator
            .GetInt32(-2_147_483_640, 2_147_483_640)
            .ToString();

        return @$"C:\Storage\{generatedName}";
    }
}
