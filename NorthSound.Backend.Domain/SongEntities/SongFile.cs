namespace NorthSound.Backend.Domain.SongEntities;

public class SongFile
{
    public SongFile() { }

    public SongFile(string name, FileStream fileStream, string contentType)
    {
        Name = name;
        FileStream = fileStream;
        ContentType = contentType;
    }

    public string Name { get; set; } = default!;
    public FileStream FileStream { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
