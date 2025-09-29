namespace AudioPool.Repository.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.Entities;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Data;
using AudioPool.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ArtistRepository(AudioPoolDbContext db) : IArtistRepository
{
    public int CreateNewArtist(ArtistInputModel inputModel)
    {
        var entity = new Artist
        {
            Name = inputModel.name!,
            Bio = inputModel.bio,
            CoverImageUrl = inputModel.coverImageUrl,
            DateOfStart = inputModel.DateOfStart,
            DateCreated = DateTime.Now,
        };
        db.Artists.Add(entity);
        db.SaveChanges();
        return entity.Id;
    }

    public void DeleteArtistById(int id)
    {
        var entity = db.Artists.FirstOrDefault(a => a.Id == id);
        if (entity != null)
        {
            db.Artists.Remove(entity);
            db.SaveChanges();
        }
    }

    public IEnumerable<ArtistDto> GetAllArtists(bool containUnavailable)
    {
        return db
            .Artists.AsNoTracking()
            .OrderByDescending(a => a.DateOfStart)
            .Select(a => new ArtistDto
            {
                id = a.Id,
                name = a.Name,
                coverImageUrl = a.CoverImageUrl,
            });
    }

    public ArtistDto? GetArtistById(int id)
    {
        var a = db.Artists.AsNoTracking().FirstOrDefault(a => a.Id == id);
        return a == null
            ? null
            : new ArtistDto
            {
                id = a.Id,
                name = a.Name,
                coverImageUrl = a.CoverImageUrl,
            };
    }

    public void UpdateArtistById(int id, ArtistInputModel inputModel)
    {
        var a = db.Artists.FirstOrDefault(a => a.Id == id);
        if (a == null)
            return;
        a.Name = inputModel.name!;
        a.Bio = inputModel.bio;
        a.CoverImageUrl = inputModel.coverImageUrl;
        a.DateOfStart = inputModel.DateOfStart;
        a.DateModified = DateTime.Now;
        a.ModifiedBy = "admin";
        db.SaveChanges();
    }

    public void UpdateArtistPartiallyById(int id, ArtistPartialInputModel inputModel)
    {
        var a = db.Artists.FirstOrDefault(a => a.Id == id);
        if (a == null)
            return;
        if (inputModel.name != null)
            a.Name = inputModel.name;
        if (inputModel.bio != null)
            a.Bio = inputModel.bio;
        if (inputModel.coverImageUrl != null)
            a.CoverImageUrl = inputModel.coverImageUrl;
        if (inputModel.DateOfStart.HasValue)
            a.DateOfStart = inputModel.DateOfStart;
        a.DateModified = DateTime.Now;
        a.ModifiedBy = "admin";
        db.SaveChanges();
    }
}
