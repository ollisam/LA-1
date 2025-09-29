namespace AudioPool.Services.Interfaces;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

public interface IArtistService
{
    IEnumerable<ArtistDto> GetArtists(int pageSize);
    ArtistDto? GetArtistById(int id);
    IEnumerable<AlbumDto> GetAlbumsForArtist(int artistId, int pageSize);

    int CreateNewArtist(ArtistInputModel inputModel);
    void UpdateArtistById(int id, ArtistInputModel inputModel);
    void UpdateArtistPartiallyById(int id, ArtistPartialInputModel inputModel);
    void DeleteArtistById(int id);
}
