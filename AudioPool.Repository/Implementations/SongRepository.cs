namespace AudioPool.Repository.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.Entities;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Data;
using AudioPool.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class SongRepository(AudioPoolDbContext db) : ISongRepository
{
    public SongDetailsDto? GetSongDetailsById(int id)
    {
        var song = db.Songs.AsNoTracking().Include(s => s.Album).FirstOrDefault(s => s.Id == id);
        if (song == null) return null;

        int trackNumber = 0;
        if (song.AlbumId.HasValue)
        {
            var orderedIds = db.Songs.AsNoTracking()
                .Where(s => s.AlbumId == song.AlbumId)
                .OrderBy(s => s.Id)
                .Select(s => s.Id)
                .ToList();
            trackNumber = orderedIds.FindIndex(x => x == id) + 1;
        }

        return new SongDetailsDto
        {
            id = song.Id,
            name = song.Name,
            duration = song.Duration,
            album = song.Album == null ? null : new AlbumDto
            {
                id = song.Album.Id,
                name = song.Album.Name,
                releaseDate = song.Album.ReleaseDate,
                coverImageUrl = song.Album.CoverImageUrl,
                description = song.Album.Description,
            },
            trackNumberOnAlbum = trackNumber,
        };
    }
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
        return db
            .Songs.AsNoTracking()
            .Select(s => new SongDto
            {
                id = s.Id,
                name = s.Name,
                duration = s.Duration,
            })
            .ToList();
    }

    public SongDto? GetSongById(int id)
    {
        return db
            .Songs.AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new SongDto
            {
                id = s.Id,
                name = s.Name,
                duration = s.Duration,
            })
            .FirstOrDefault();
    }

    public void UpdateSongById(int id, SongInputModel inputModel)
    {
        var song = db.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null)
            return;

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
        if (song == null)
            return;

        if (inputModel.name != null)
            song.Name = inputModel.name;
        if (inputModel.duration != null)
            song.Duration = inputModel.duration.Value;
        if (inputModel.albumId.HasValue)
            song.AlbumId = inputModel.albumId;

        song.ModifiedBy = "admin";
        song.DateModified = DateTime.Now;
        db.SaveChanges();
    }

    public void DeleteSongById(int id)
    {
        var song = db.Songs.FirstOrDefault(s => s.Id == id);
        if (song == null)
            return;
        db.Songs.Remove(song);
        db.SaveChanges();
    }

    public IEnumerable<SongDto> GetSongsByAlbumId(int albumId)
    {
        return db
            .Songs.AsNoTracking()
            .Where(s => s.AlbumId == albumId)
            .Select(s => new SongDto
            {
                id = s.Id,
                name = s.Name,
                duration = s.Duration,
            })
            .ToList();
    }
}
