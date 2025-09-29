namespace AudioPool.Services.Interfaces;

using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;

public interface IGenreService
{
    IEnumerable<GenreDto> GetAllGenres();
    GenreDto? GetGenreById(int id);

    int CreateNewGenre(GenreInputModel inputModel);
    void UpdateGenreById(int id, GenreInputModel inputModel);
    void UpdateGenrePartiallyById(int id, GenrePartialInputModel inputModel);
    void DeleteGenreById(int id);
}
