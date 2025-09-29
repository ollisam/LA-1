namespace AudioPool.Models.Entities;

public class Song
{
    // id of the song
    public int Id { get; set; }

    // name of the song
    public string Name { get; set; } = null!;

    // duration of the song
    public TimeSpan Duration { get; set; }

    // FK to album (optional for now to not break existing inputs)
    public int? AlbumId { get; set; }
    public Album? Album { get; set; }

    // creation time of the song
    public DateTime DateCreated { get; set; }

    // modification time of the song
    public DateTime? DateModified { get; set; }

    // modified by of the song e.g owner of the song
    public string? ModifiedBy { get; set; }
}
