namespace AudioPool.Models.InputModels;

using System.ComponentModel.DataAnnotations;

public class AlbumInputModel
{
    [Required]
    [MinLength(3)]
    public string name { get; set; } = null!;

    [Required]
    public DateTime? releaseDate { get; set; }

    [Required]
    public IEnumerable<int> artistIds { get; set; } = Array.Empty<int>();

    [Url]
    public string? coverImageUrl { get; set; }

    [MinLength(10)]
    public string? description { get; set; }
}
