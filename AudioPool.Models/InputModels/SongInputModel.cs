namespace AudioPool.Models.InputModels;

public class SongInputModel
{
    // name of the song
    public string name { get; set; } = null!;

    // duration of the song
    public TimeSpan duration { get; set; }

    // optional album id
    public int? albumId { get; set; }
}
