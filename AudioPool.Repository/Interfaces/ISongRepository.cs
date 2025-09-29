using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

namespace AudioPool.Repository.Interfaces;

public interface ISongRepository
{
    IEnumerable<SongDto> GetAllSongs(bool containUnavailable);
    SongDto? GetSongById(int id);
    SongDetailsDto? GetSongDetailsById(int id);
    int CreateNewSong(SongInputModel inputModel);
    void UpdateSongById(int id, SongInputModel inputModel);
    void UpdateSongPartiallyById(int id, SongPartialInputModel inputModel);
    void DeleteSongById(int id);
    IEnumerable<SongDto> GetSongsByAlbumId(int albumId);
}
