namespace AudioPool.Models.InputModels;

public class SongPartialInputModel
{
    // name of the song
    public string? name { get; set; } = null!;

    // duration of the song
    public int? duration { get; set; }
}
