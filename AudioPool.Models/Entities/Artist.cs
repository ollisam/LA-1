namespace AudioPool.Models.Entities;

public class Artist
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Bio { get; set; }
    public string? CoverImageUrl { get; set; }
    public DateTime? DateOfStart { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }

    public ICollection<Album> Albums { get; set; } = new List<Album>();
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
