namespace AudioPool.Models.Dtos;

public class ArtistDto
{
    public int id { get; set; }
    public string name { get; set; } = null!;
    public string? coverImageUrl { get; set; }
    public string? bio { get; set; }
    public DateTime dateOfStart { get; set; }
}
