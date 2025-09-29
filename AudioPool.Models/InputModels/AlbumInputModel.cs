namespace AudioPool.Models.InputModels;

using System.ComponentModel.DataAnnotations;

public class AlbumInputModel
{
    public string name { get; set; } = null!;
    [Required]
    public DateTime? releaseDate { get; set; }
    public string? coverImageUrl { get; set; }
    public string? description { get; set; }
}
