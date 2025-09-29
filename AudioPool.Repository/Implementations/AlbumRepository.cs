namespace AudioPool.Repository.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.Entities;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Data;
using AudioPool.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class AlbumRepository(AudioPoolDbContext db) : IAlbumRepository
{
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
        return db.Albums.AsNoTracking().Select(a => new AlbumDto
        {
            id = a.Id,
            name = a.Name,
            releaseDate = a.ReleaseDate,
            coverImageUrl = a.CoverImageUrl,
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
        return db.Albums
            .AsNoTracking()
            .Where(a => a.Artists.Any(ar => ar.Id == artistId))
            .Select(a => new AlbumDto
            {
                id = a.Id,
                name = a.Name,
                releaseDate = a.ReleaseDate,
                coverImageUrl = a.CoverImageUrl
            })
            .ToList();
    }
}
