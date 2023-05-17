namespace NorthSound.Backend.Services.Other;

public interface IStorageGenerator
{
    /// <summary>
    /// Генерирует путь файла к хранилищу
    /// </summary>
    /// <returns>Строка в формате "disk:/storage/file"</returns>
    string GetNewGeneratedPath();
}
