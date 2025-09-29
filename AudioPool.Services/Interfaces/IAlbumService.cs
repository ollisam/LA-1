namespace AudioPool.Services.Interfaces;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

public interface IAlbumService
{
    IEnumerable<AlbumDto> GetAlbums(int pageSize);
    AlbumDto? GetAlbumById(int id);
    IEnumerable<SongDto> GetSongsOnAlbum(int albumId, int pageSize);

    int CreateNewAlbum(AlbumInputModel inputModel);
    void UpdateAlbumById(int id, AlbumInputModel inputModel);
    void UpdateAlbumPartiallyById(int id, AlbumPartialInputModel inputModel);
    void DeleteAlbumById(int id);
}
