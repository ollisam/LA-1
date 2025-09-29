using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

namespace AudioPool.Repository.Interfaces;

public interface IArtistRepository
{
    IEnumerable<ArtistDto> GetAllArtists(bool containUnavailable);
    ArtistDto? GetArtistById(int id);
    ArtistDetailsDto? GetArtistDetailsById(int id);
    void LinkArtistToGenre(int artistId, int genreId);
    IEnumerable<ArtistDto> GetArtistsByGenreId(int genreId);
    int CreateNewArtist(ArtistInputModel inputModel);
    void UpdateArtistById(int id, ArtistInputModel inputModel);
    void UpdateArtistPartiallyById(int id, ArtistPartialInputModel inputModel);
    void DeleteArtistById(int id);
}
