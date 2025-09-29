using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;
    private readonly IArtistService _artistService;

    public GenresController(IGenreService service, IArtistService artistService)
    {
        _service = service;
        _artistService = artistService;
    }

    [HttpGet(Name = "GetAllGenres")]
    [AllowAnonymous]
    public ActionResult GetAll()
    {
        var genres = _service.GetAllGenres().ToList();
        var shaped = genres.Select(g => new
        {
            g.id,
            g.name,
            _links = new
            {
                self = new { href = $"/api/genres/{g.id}" },
                artists = _artistService.GetArtistsByGenreId(g.id).Select(a => new { href = $"/api/artists/{a.id}" })
            }
        });
        return Ok(shaped);
    }

    [HttpGet("{id:int}", Name = "GetGenreById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var dto = _service.GetGenreById(id);
        if (dto is null) return NotFound();

        var artists = _artistService.GetArtistsByGenreId(id).ToList();
        var response = new
        {
            dto.id,
            dto.name,
            numberOfArtists = artists.Count,
            _links = new
            {
                self = new { href = $"/api/genres/{id}" },
                artists = artists.Select(a => new { href = $"/api/artists/{a.id}" })
            }
        };

        return Ok(response);
    }

    [HttpPost]
    [ApiTokenAuthorize]
    public ActionResult Create([FromBody] GenreInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _service.CreateNewGenre(input);
        var dto = _service.GetGenreById(id);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult Update(int id, [FromBody] GenreInputModel input)
    {
        var exists = _service.GetGenreById(id) != null;
        if (!exists)
            return NotFound();
        _service.UpdateGenreById(id, input);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult UpdatePartially(int id, [FromBody] GenrePartialInputModel input)
    {
        var exists = _service.GetGenreById(id) != null;
        if (!exists)
            return NotFound();
        _service.UpdateGenrePartiallyById(id, input);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult Delete(int id)
    {
        var exists = _service.GetGenreById(id) != null;
        if (!exists)
            return NotFound();
        _service.DeleteGenreById(id);
        return NoContent();
    }
}
