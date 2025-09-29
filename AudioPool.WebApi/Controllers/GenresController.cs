using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreRepository _repository;

    public GenresController(IGenreRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(Name = "GetAllGenres")]
    public ActionResult GetAll()
    {
        var list = _repository.GetAllGenres(false).ToList();
        return Ok(list);
    }

    [HttpGet("{id:int}", Name = "GetGenreById")]
    public ActionResult GetById(int id)
    {
        var dto = _repository.GetGenreById(id);
        if (dto is null)
            return NotFound();
        return Ok(MapToResource(dto));
    }

    [HttpPost]
    public ActionResult Create([FromBody] GenreInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _repository.CreateNewGenre(input);
        var dto = _repository.GetGenreById(id);
        var resource = dto is null ? null : MapToResource(dto);
        return CreatedAtAction(nameof(GetById), new { id }, resource);
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] GenreInputModel input)
    {
        var exists = _repository.GetGenreById(id) != null;
        if (!exists)
            return NotFound();
        _repository.UpdateGenreById(id, input);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public ActionResult UpdatePartially(int id, [FromBody] GenrePartialInputModel input)
    {
        var exists = _repository.GetGenreById(id) != null;
        if (!exists)
            return NotFound();
        _repository.UpdateGenrePartiallyById(id, input);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var exists = _repository.GetGenreById(id) != null;
        if (!exists)
            return NotFound();
        _repository.DeleteGenreById(id);
        return NoContent();
    }

    private object MapToResource(GenreDto dto)
    {
        return new
        {
            dto.id,
            dto.name,
            _links = new { self = new LinkRepresentation { Href = BuildGenreUrl(dto.id) } },
        };
    }

    private string BuildGenreUrl(int id) => Url.Link("GetGenreById", new { id })!;
}
