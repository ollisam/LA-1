namespace AudioPool.Services.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using AudioPool.Services.Interfaces;

public class SongService : ISongService
{
    private readonly ISongRepository _songRepository;

    public SongService(ISongRepository songRepository)
    {
        _songRepository = songRepository;
    }

    public IEnumerable<SongDto> GetAllSongs(bool containUnavailable) =>
        _songRepository.GetAllSongs(containUnavailable);

    public SongDto? GetSongById(int id) => _songRepository.GetSongById(id);
    public SongDetailsDto? GetSongDetailsById(int id) => _songRepository.GetSongDetailsById(id);

    public int CreateNewSong(SongInputModel inputModel) =>
        _songRepository.CreateNewSong(inputModel);

    public void UpdateSongById(int id, SongInputModel inputModel) =>
        _songRepository.UpdateSongById(id, inputModel);

    public void UpdateSongPartiallyById(int id, SongPartialInputModel inputModel) =>
        _songRepository.UpdateSongPartiallyById(id, inputModel);

    public void DeleteSongById(int id) => _songRepository.DeleteSongById(id);
}
