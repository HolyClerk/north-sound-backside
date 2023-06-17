using NorthSound.Backend.Services.Other;
using System.Reflection;
using System.Security.Cryptography;

namespace NorthSound.Backend.Infrastructure;

public class Locator : ILocator
{
    public Uri GeneratePath()
    {
        string generatedName = RandomNumberGenerator
            .GetInt32(-2_147_483_640, 2_147_483_640)
            .ToString();

        var workPath = GetWorkPath();

        return new Uri(Path.Combine(workPath, generatedName));
    }

    public async Task LocateAsync(Stream stream, string generatedPath)
    {
        using (var filestream = new FileStream(generatedPath, FileMode.Create))
        {
            await stream.CopyToAsync(filestream);
            await stream.DisposeAsync();
        }
    }

    public string GetWorkPath()
    {
        string executionPath = Assembly.GetExecutingAssembly().Location;
        return Path.GetDirectoryName(executionPath)! + "/music/";
    }
}
