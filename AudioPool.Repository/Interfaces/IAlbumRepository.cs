using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

namespace AudioPool.Repository.Interfaces;

public interface IAlbumRepository
{
    IEnumerable<AlbumDto> GetAllAlbums(bool containUnavailable);
    AlbumDto? GetAlbumById(int id);
    int CreateNewAlbum(AlbumInputModel inputModel);
    void UpdateAlbumById(int id, AlbumInputModel inputModel);
    void UpdateAlbumPartiallyById(int id, AlbumPartialInputModel inputModel);
    void DeleteAlbumById(int id);
}
