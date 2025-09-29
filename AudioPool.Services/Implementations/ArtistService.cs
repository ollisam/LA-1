namespace AudioPool.Services.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using AudioPool.Services.Interfaces;

public class ArtistService : IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IAlbumRepository _albumRepository;

    public ArtistService(IArtistRepository artistRepository, IAlbumRepository albumRepository)
    {
        _artistRepository = artistRepository;
        _albumRepository = albumRepository;
    }

    public IEnumerable<ArtistDto> GetArtists(int pageSize)
    {
        if (pageSize < 1)
            pageSize = 25;
        return _artistRepository.GetAllArtists(false).Take(pageSize);
    }

    public ArtistDto? GetArtistById(int id) => _artistRepository.GetArtistById(id);
    public ArtistDetailsDto? GetArtistDetailsById(int id) => _artistRepository.GetArtistDetailsById(id);

    public IEnumerable<AlbumDto> GetAlbumsForArtist(int artistId, int pageSize)
    {
        if (pageSize < 1)
            pageSize = 25;
        return _albumRepository.GetAlbumsByArtistId(artistId).Take(pageSize);
    }

    public void LinkArtistToGenre(int artistId, int genreId) =>
        _artistRepository.LinkArtistToGenre(artistId, genreId);

    public IEnumerable<ArtistDto> GetArtistsByGenreId(int genreId) =>
        _artistRepository.GetArtistsByGenreId(genreId);

    public int CreateNewArtist(ArtistInputModel inputModel) =>
        _artistRepository.CreateNewArtist(inputModel);

    public void UpdateArtistById(int id, ArtistInputModel inputModel) =>
        _artistRepository.UpdateArtistById(id, inputModel);

    public void UpdateArtistPartiallyById(int id, ArtistPartialInputModel inputModel) =>
        _artistRepository.UpdateArtistPartiallyById(id, inputModel);

    public void DeleteArtistById(int id) => _artistRepository.DeleteArtistById(id);
}
