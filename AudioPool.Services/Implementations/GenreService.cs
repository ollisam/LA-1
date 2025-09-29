namespace AudioPool.Services.Implementations;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using AudioPool.Services.Interfaces;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public IEnumerable<GenreDto> GetAllGenres() => _genreRepository.GetAllGenres(false);

    public GenreDto? GetGenreById(int id) => _genreRepository.GetGenreById(id);

    public int CreateNewGenre(GenreInputModel inputModel) =>
        _genreRepository.CreateNewGenre(inputModel);

    public void UpdateGenreById(int id, GenreInputModel inputModel) =>
        _genreRepository.UpdateGenreById(id, inputModel);

    public void UpdateGenrePartiallyById(int id, GenrePartialInputModel inputModel) =>
        _genreRepository.UpdateGenrePartiallyById(id, inputModel);

    public void DeleteGenreById(int id) => _genreRepository.DeleteGenreById(id);
}
