namespace AudioPool.Models.InputModels;

using System.ComponentModel.DataAnnotations;

public class ArtistInputModel
{
    [Required]
    [MinLength(3)]
    public string name { get; set; } = null!;

    [MinLength(10)]
    public string? bio { get; set; }

    [Url]
    public string? coverImageUrl { get; set; }

    [Required]
    public DateTime? DateOfStart { get; set; }
}
