namespace NorthSound.Backend.Services.Other;

public interface ILocator
{
    /// <summary>
    /// Генерирует путь файла к хранилищу
    /// </summary>
    /// <returns>Строка в формате "disk:/storage/file"</returns>
    Uri GeneratePath();
    string GetWorkPath();
    Task LocateAsync(Stream stream, string generatedPath);
}
