namespace AudioPool.Repository.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.Entities;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Data;
using AudioPool.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class GenreRepository(AudioPoolDbContext db) : IGenreRepository
{
    public int CreateNewGenre(GenreInputModel inputModel)
    {
        var entity = new Genre { Name = inputModel.name!, DateCreated = DateTime.Now };
        db.Genres.Add(entity);
        db.SaveChanges();
        return entity.Id;
    }

    public void DeleteGenreById(int id)
    {
        var entity = db.Genres.FirstOrDefault(g => g.Id == id);
        if (entity != null)
        {
            db.Genres.Remove(entity);
            db.SaveChanges();
        }
    }

    public IEnumerable<GenreDto> GetAllGenres(bool containUnavailable)
    {
        return db.Genres.AsNoTracking().Select(g => new GenreDto { id = g.Id, name = g.Name });
    }

    public GenreDto? GetGenreById(int id)
    {
        var g = db.Genres.AsNoTracking().FirstOrDefault(g => g.Id == id);
        return g == null ? null : new GenreDto { id = g.Id, name = g.Name };
    }

    public void UpdateGenreById(int id, GenreInputModel inputModel)
    {
        var g = db.Genres.FirstOrDefault(g => g.Id == id);
        if (g == null)
            return;
        g.Name = inputModel.name!;
        g.DateModified = DateTime.Now;
        g.ModifiedBy = "admin";
        db.SaveChanges();
    }

    public void UpdateGenrePartiallyById(int id, GenrePartialInputModel inputModel)
    {
        var g = db.Genres.FirstOrDefault(g => g.Id == id);
        if (g == null)
            return;
        if (inputModel.name != null)
            g.Name = inputModel.name;
        g.DateModified = DateTime.Now;
        g.ModifiedBy = "admin";
        db.SaveChanges();
    }
}
