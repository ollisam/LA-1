using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

namespace AudioPool.Repository.Interfaces;

public interface IGenreRepository
{
    IEnumerable<GenreDto> GetAllGenres(bool containUnavailable);
    GenreDto? GetGenreById(int id);
    int CreateNewGenre(GenreInputModel inputModel);
    void UpdateGenreById(int id, GenreInputModel inputModel);
    void UpdateGenrePartiallyById(int id, GenrePartialInputModel inputModel);
    void DeleteGenreById(int id);
}
