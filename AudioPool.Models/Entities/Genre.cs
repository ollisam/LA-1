namespace AudioPool.Models.Entities;

public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime DateCreated { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }

    public ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
