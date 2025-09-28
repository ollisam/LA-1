using AudioPool.Models.Entities;

namespace AudioPool.Repository.Data;

public class DataProvider
{
    private static readonly string _adminName = "SongAdmin";
    public List<Song> Songs { get; set; } =
        new()
        {
            new Song
            {
                id = 1,
                name = "Bohemian Rhapsody",
                duration = 354,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ModifiedBy = _adminName,
            },
            new Song
            {
                id = 2,
                name = "Stairway to Heaven",
                duration = 482,
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
            new Song
            {
                id = 3,
                name = "Hotel California",
                duration = 391,
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
        };
}
