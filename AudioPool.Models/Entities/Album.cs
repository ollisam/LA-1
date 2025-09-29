namespace AudioPool.Models.Entities;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime? ReleaseDate { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Description { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }

    public ICollection<Song> Songs { get; set; } = new List<Song>();
    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
