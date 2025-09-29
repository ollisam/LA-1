namespace AudioPool.Repository.Data;

using AudioPool.Models.Entities;

public class DataProvider
{
    private static readonly string _adminName = "SongAdmin";
    public List<Artist> Artists { get; set; } =
        new()
        {
            new Artist
            {
                Id = 1,
                Name = "Daft Punk",
                Bio = "French electronic music duo",
                CoverImageUrl = "https://example.com/daft-punk.jpg",
                DateOfStart = new DateTime(1993, 1, 1),
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
            new Artist
            {
                Id = 2,
                Name = "Elvis Presley",
                Bio = "King of Rock and Roll",
                CoverImageUrl = "https://example.com/elvis.jpg",
                DateOfStart = new DateTime(1954, 7, 5),
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
        };

    public List<Album> Albums { get; set; } =
        new()
        {
            new Album
            {
                Id = 1,
                Name = "Discovery",
                ReleaseDate = new DateTime(2001, 3, 12),
                CoverImageUrl = "https://example.com/discovery.jpg",
                Description = "Daft Punk second studio album",
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
            new Album
            {
                Id = 2,
                Name = "Random Access Memories",
                ReleaseDate = new DateTime(2013, 5, 17),
                CoverImageUrl = "https://example.com/ram.jpg",
                Description = "Daft Punk final studio album",
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
        };

    public List<Genre> Genres { get; set; } =
        new()
        {
            new Genre
            {
                Id = 1,
                Name = "Electronic",
                DateCreated = DateTime.Now,
            },
            new Genre
            {
                Id = 2,
                Name = "Rock",
                DateCreated = DateTime.Now,
            },
            new Genre
            {
                Id = 3,
                Name = "Pop",
                DateCreated = DateTime.Now,
            },
        };
    public List<Song> Songs { get; set; } =
        new()
        {
            new Song
            {
                Id = 1,
                Name = "Bohemian Rhapsody",
                Duration = TimeSpan.FromSeconds(354),
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                ModifiedBy = _adminName,
            },
            new Song
            {
                Id = 2,
                Name = "Stairway to Heaven",
                Duration = TimeSpan.FromSeconds(482),
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
            new Song
            {
                Id = 3,
                Name = "Hotel California",
                Duration = TimeSpan.FromSeconds(391),
                DateCreated = DateTime.Now,
                DateModified = null,
                ModifiedBy = null,
            },
        };
}
