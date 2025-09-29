namespace AudioPool.Repository.Implementations;

using AudioPool.Repository.Interfaces;
using AudioPool.Repository.Data;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Models.Entities;
using Microsoft.EntityFrameworkCore;

public class SongRepository(AudioPoolDbContext db) : ISongRepository
{
    public int CreateNewSong(SongInputModel inputModel)
    {
        var entity = new Song
        {
            Name = inputModel.name,
            Duration = inputModel.duration,
            AlbumId = inputModel.albumId,
            DateCreated = DateTime.Now,
        };
        db.Songs.Add(entity);
        db.SaveChanges();
        return entity.Id;
    }

    public IEnumerable<SongDto> GetAllSongs(bool containUnavailable)
    {
        return db.Songs
            .AsNoTracking()
            .Select(s => new SongDto
            {
                id = s.Id,
                name = s.Name,
                duration = s.Duration,
                albumId = s.AlbumId
            })
            .ToList();
    }

    public SongDto? GetSongById(int id)
    {
        return db.Songs
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SongDto
            {
                id = s.Id,
                name = s.Name,
                duration = s.Duration,
                albumId = s.AlbumId
            })
            .FirstOrDefault();
    }

    public void UpdateSongById(int id, SongInputModel inputModel)
    {
        var song = db.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null) return;

        song.Name = inputModel.name;
        song.Duration = inputModel.duration;
        song.AlbumId = inputModel.albumId;
        song.ModifiedBy = "admin";
        song.DateModified = DateTime.Now;
        db.SaveChanges();
    }

    public void UpdateSongPartiallyById(int id, SongPartialInputModel inputModel)
    {
        var song = db.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null) return;

        if (inputModel.name != null) song.Name = inputModel.name;
        if (inputModel.duration != null) song.Duration = inputModel.duration.Value;
        if (inputModel.albumId.HasValue) song.AlbumId = inputModel.albumId;

        song.ModifiedBy = "admin";
        song.DateModified = DateTime.Now;
        db.SaveChanges();
    }

    public void DeleteSongById(int id)
    {
        var song = db.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null) return;
        db.Songs.Remove(song);
        db.SaveChanges();
    }
}
