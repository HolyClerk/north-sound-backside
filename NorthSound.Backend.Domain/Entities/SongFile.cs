namespace NorthSound.Backend.Domain.Entities;

public class SongFile
{
    public SongFile() { }

    public SongFile(string name, FileStream fileStream, string contentType)
    {
        Name = name;
        FileStream = fileStream;
        ContentType = contentType;
    }

    public string Name { get; set; }
    public FileStream FileStream { get; set; }
    public string ContentType { get; set; }
}
