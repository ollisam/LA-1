namespace AudioPool.Models.InputModels;

using System.ComponentModel.DataAnnotations;

public class SongInputModel
{
    [Required]
    [MinLength(3)]
    public string name { get; set; } = null!;

    [Required]
    public TimeSpan duration { get; set; }
    
    [Required]
    public int albumId { get; set; }
}
