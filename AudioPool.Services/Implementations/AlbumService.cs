namespace AudioPool.Services.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using AudioPool.Services.Interfaces;

public class AlbumService : IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly ISongRepository _songRepository;

    public AlbumService(IAlbumRepository albumRepository, ISongRepository songRepository)
    {
        _albumRepository = albumRepository;
        _songRepository = songRepository;
    }

    public AlbumDto? GetAlbumById(int id) => _albumRepository.GetAlbumById(id);
    public AlbumDetailsDto? GetAlbumDetailsById(int id) => _albumRepository.GetAlbumDetailsById(id);

    public IEnumerable<AlbumDto> GetAlbums(int pageSize)
    {
        if (pageSize < 1)
            pageSize = 25;
        return _albumRepository.GetAllAlbums(false).Take(pageSize);
    }

    public IEnumerable<SongDto> GetSongsOnAlbum(int albumId, int pageSize)
    {
        if (pageSize < 1)
            pageSize = 25;
        return _songRepository.GetSongsByAlbumId(albumId).Take(pageSize);
    }

    public int CreateNewAlbum(AlbumInputModel inputModel) =>
        _albumRepository.CreateNewAlbum(inputModel);

    public void UpdateAlbumById(int id, AlbumInputModel inputModel) =>
        _albumRepository.UpdateAlbumById(id, inputModel);

    public void UpdateAlbumPartiallyById(int id, AlbumPartialInputModel inputModel) =>
        _albumRepository.UpdateAlbumPartiallyById(id, inputModel);

    public void DeleteAlbumById(int id) => _albumRepository.DeleteAlbumById(id);
}
