namespace AudioPool.Models.InputModels;

public class ArtistInputModel
{
    public string name { get; set; } = null!;
    public string? bio { get; set; }
    public string? coverImageUrl { get; set; }
    public DateTime? DateOfStart { get; set; }
}
