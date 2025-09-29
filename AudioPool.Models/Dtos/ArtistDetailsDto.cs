namespace AudioPool.Models.Dtos;

public class ArtistDetailsDto
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public string? bio { get; set; }
    public string? coverImageUrl { get; set; }
    public DateTime? dateOfStart { get; set; }

    public IEnumerable<AlbumDto> albums { get; set; } = Enumerable.Empty<AlbumDto>();
    public IEnumerable<GenreDto> genres { get; set; } = Enumerable.Empty<GenreDto>();
}

