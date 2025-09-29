namespace AudioPool.Models.Dtos;

public class AlbumDetailsDto
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public DateTime? releaseDate { get; set; }
    public string? coverImageUrl { get; set; }
    public string? description { get; set; }

    public IEnumerable<ArtistDto> artists { get; set; } = Enumerable.Empty<ArtistDto>();
    public IEnumerable<SongDto> songs { get; set; } = Enumerable.Empty<SongDto>();
}
