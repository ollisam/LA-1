namespace AudioPool.Repository.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.Entities;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Data;
using AudioPool.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AlbumRepository(AudioPoolDbContext db) : IAlbumRepository
{
    public AlbumDetailsDto? GetAlbumDetailsById(int id)
    {
        var album = db.Albums.AsNoTracking()
            .Include(a => a.Artists)
            .Include(a => a.Songs)
            .FirstOrDefault(a => a.Id == id);
        if (album == null) return null;

        var artists = album.Artists.Select(ar => new ArtistDto
        {
            id = ar.Id,
            name = ar.Name,
            coverImageUrl = ar.CoverImageUrl,
            bio = ar.Bio,
            dateOfStart = ar.DateOfStart,
        }).ToList();

        var songs = album.Songs.OrderBy(s => s.Id).Select(s => new SongDto
        {
            id = s.Id,
            name = s.Name,
            duration = s.Duration,
            albumId = s.AlbumId
        }).ToList();

        return new AlbumDetailsDto
        {
            id = album.Id,
            name = album.Name,
            releaseDate = album.ReleaseDate,
            coverImageUrl = album.CoverImageUrl,
            description = album.Description,
            artists = artists,
            songs = songs,
        };
    }
    public int CreateNewAlbum(AlbumInputModel inputModel)
    {
        var entity = new Album
        {
            Name = inputModel.name!,
            ReleaseDate = inputModel.releaseDate,
            CoverImageUrl = inputModel.coverImageUrl,
            Description = inputModel.description,
            DateCreated = DateTime.Now,
        };

        // Link artists if provided
        if (inputModel.artistIds != null && inputModel.artistIds.Any())
        {
            var artists = db.Artists.Where(a => inputModel.artistIds.Contains(a.Id)).ToList();
            foreach (var ar in artists)
            {
                entity.Artists.Add(ar);
            }
        }

        db.Albums.Add(entity);
        db.SaveChanges();
        return entity.Id;
    }

    public void DeleteAlbumById(int id)
    {
        var entity = db.Albums.FirstOrDefault(a => a.Id == id);
        if (entity != null)
        {
            db.Albums.Remove(entity);
            db.SaveChanges();
        }
    }

    public IEnumerable<AlbumDto> GetAllAlbums(bool containUnavailable)
    {
        return db
            .Albums.AsNoTracking()
            .Select(a => new AlbumDto
            {
                id = a.Id,
                name = a.Name,
                releaseDate = a.ReleaseDate,
                coverImageUrl = a.CoverImageUrl,
                description = a.Description,
            });
    }

    public AlbumDto? GetAlbumById(int id)
    {
        var a = db.Albums.AsNoTracking().FirstOrDefault(a => a.Id == id);
        return a == null
            ? null
            : new AlbumDto
            {
                id = a.Id,
                name = a.Name,
                releaseDate = a.ReleaseDate,
                coverImageUrl = a.CoverImageUrl,
                description = a.Description,
            };
    }

    public void UpdateAlbumById(int id, AlbumInputModel inputModel)
    {
        var a = db.Albums.FirstOrDefault(a => a.Id == id);
        if (a == null)
            return;
        a.Name = inputModel.name!;
        a.ReleaseDate = inputModel.releaseDate;
        a.CoverImageUrl = inputModel.coverImageUrl;
        a.Description = inputModel.description;
        a.DateModified = DateTime.Now;
        a.ModifiedBy = "admin";
        db.SaveChanges();
    }

    public void UpdateAlbumPartiallyById(int id, AlbumPartialInputModel inputModel)
    {
        var a = db.Albums.FirstOrDefault(a => a.Id == id);
        if (a == null)
            return;
        if (inputModel.name != null)
            a.Name = inputModel.name;
        if (inputModel.releaseDate.HasValue)
            a.ReleaseDate = inputModel.releaseDate;
        if (inputModel.coverImageUrl != null)
            a.CoverImageUrl = inputModel.coverImageUrl;
        if (inputModel.description != null)
            a.Description = inputModel.description;
        a.DateModified = DateTime.Now;
        a.ModifiedBy = "admin";
        db.SaveChanges();
    }

    public IEnumerable<AlbumDto> GetAlbumsByArtistId(int artistId)
    {
        return db
            .Albums.AsNoTracking()
            .Where(a => a.Artists.Any(ar => ar.Id == artistId))
            .Select(a => new AlbumDto
            {
                id = a.Id,
                name = a.Name,
                releaseDate = a.ReleaseDate,
                coverImageUrl = a.CoverImageUrl,
                description = a.Description,
            })
            .ToList();
    }
}
