namespace AudioPool.Models.InputModels;

public class AlbumInputModel
{
    public string name { get; set; } = null!;
    public DateTime? releaseDate { get; set; }
    public string? coverImageUrl { get; set; }
    public string? description { get; set; }
}
