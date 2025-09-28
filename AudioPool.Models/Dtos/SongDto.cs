namespace AudioPool.Models.Dtos;

public class SongDto
{
    // id of the song
    public int id { get; set; }

    // name of the song
    public string name { get; set; } = null!;

    // duration of the song
    public int duration { get; set; }
}
