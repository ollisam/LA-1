using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreService _service;

    public GenresController(IGenreService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAllGenres")]
    [AllowAnonymous]
    public ActionResult GetAll()
    {
        return Ok(_service.GetAllGenres());
    }

    [HttpGet("{id:int}", Name = "GetGenreById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var dto = _service.GetGenreById(id);
        return dto is null ? NotFound() : Ok(dto);
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
