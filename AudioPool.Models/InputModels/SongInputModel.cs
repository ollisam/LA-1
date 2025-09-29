namespace AudioPool.Models.InputModels;

using System.ComponentModel.DataAnnotations;

public class SongInputModel
{
    // name of the song
    [Required]
    [MinLength(3)]
    public string name { get; set; } = null!;

    // duration of the song
    [Required]
    public TimeSpan duration { get; set; }

    // album id
    [Required]
    public int albumId { get; set; }
}
