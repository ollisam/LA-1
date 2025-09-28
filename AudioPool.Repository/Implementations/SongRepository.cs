namespace AudioPool.Repository.Implementations;

using AudioPool.Repository.Interfaces;
using AudioPool.Repository.Data;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Models.Entities;
using System.Linq;

public class SongRepository(DataProvider dataProvider) : ISongRepository
{
    public int CreateNewSong(SongInputModel inputModel)
    {
        var next_id = dataProvider.Songs.Max(m => m.id) + 1;
        var entity = new Song
        {
            id = next_id,
            name = inputModel.name,
            duration = inputModel.duration,
            DateCreated = DateTime.Now,
        };
        dataProvider.Songs.Add(entity);
        return next_id;
    }

    public IEnumerable<SongDto> GetAllSongs(bool containUnavailable)
    {
        return dataProvider
            .Songs
            .Select(s => new SongDto
            {
                id = s.id,
                name = s.name,
                duration = s.duration,
            });
    }

    public SongDto? GetSongById(int id)
    {
        var song = dataProvider.Songs.FirstOrDefault(s => s.id == id);
        if (song == null)
        {
            return null;
        }
        return new SongDto
        {
            id = song.id,
            name = song.name,
            duration = song.duration,
        };
    }

    public void UpdateSongById(int id, SongInputModel inputModel)
    {
        var song = dataProvider.Songs.FirstOrDefault(s => s.id == id);
        if (song == null)
        {
            return;
        }

        // update properties
        song.name = inputModel.name;
        song.duration = inputModel.duration;
        song.ModifiedBy = "admin";
        song.DateModified = DateTime.Now;
    }

    public void UpdateSongPartiallyById(int id, SongPartialInputModel inputModel)
    {
        var song = dataProvider.Songs.FirstOrDefault(s => s.id == id);
        if (song == null)
        {
            return;
        }

        if (inputModel.name != null)
        {
            song.name = inputModel.name;
        }

        if (inputModel.duration != null)
        {
            song.duration = inputModel.duration.Value;
        }

        song.ModifiedBy = "admin";
        song.DateModified = DateTime.Now;
    }

   public void DeleteSongById(int id)
    {
        var song = dataProvider.Songs.FirstOrDefault(s => s.id == id);
        if (song == null)
        {
            return;
        }
        dataProvider.Songs.Remove(song);
    }
}
