namespace AudioPool.Models.Dtos;

public class SongDto
{
    // id of the song
    public int id { get; set; }

    // name of the song
    public string name { get; set; } = null!;

    // duration of the song
    public TimeSpan duration { get; set; }

    // album id (nullable)
    public int? albumId { get; set; }
}
