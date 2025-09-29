namespace AudioPool.Services.Interfaces;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

public interface ISongService
{
    IEnumerable<SongDto> GetAllSongs(bool containUnavailable);
    SongDto? GetSongById(int id);
    int CreateNewSong(SongInputModel inputModel);
    void UpdateSongById(int id, SongInputModel inputModel);
    void UpdateSongPartiallyById(int id, SongPartialInputModel inputModel);
    void DeleteSongById(int id);
}
