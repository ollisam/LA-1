namespace AudioPool.Models.Dtos;

public class SongDetailsDto
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public TimeSpan duration { get; set; }

    public AlbumDto? album { get; set; }
    public int trackNumberOnAlbum { get; set; }
}

