namespace AudioPool.Models.Dtos;

public class AlbumDto
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public DateTime? releaseDate { get; set; }
    public string? coverImageUrl { get; set; }
}
