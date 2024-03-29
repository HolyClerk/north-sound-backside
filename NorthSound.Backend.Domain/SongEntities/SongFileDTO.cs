﻿namespace NorthSound.Backend.Domain.SongEntities;

public class SongFileDTO
{
    public SongFileDTO() { }

    public SongFileDTO(string name, FileStream fileStream, string contentType)
    {
        Name = name;
        FileStream = fileStream;
        ContentType = contentType;
    }

    public string Name { get; set; } = default!;
    public FileStream FileStream { get; set; } = default!;
    public string ContentType { get; set; } = default!;
}
