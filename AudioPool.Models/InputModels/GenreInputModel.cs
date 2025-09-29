namespace AudioPool.Models.InputModels;

using System.ComponentModel.DataAnnotations;

public class GenreInputModel
{
    [Required]
    [MinLength(3)]
    public string name { get; set; } = null!;
}
