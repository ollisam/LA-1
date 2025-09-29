using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

namespace AudioPool.Repository.Interfaces;

public interface IArtistRepository
{
    IEnumerable<ArtistDto> GetAllArtists(bool containUnavailable);
    ArtistDto? GetArtistById(int id);
    int CreateNewArtist(ArtistInputModel inputModel);
    void UpdateArtistById(int id, ArtistInputModel inputModel);
    void UpdateArtistPartiallyById(int id, ArtistPartialInputModel inputModel);
    void DeleteArtistById(int id);
}
