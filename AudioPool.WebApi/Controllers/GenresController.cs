using AudioPool.Models;
using AudioPool.Models.Dtos;
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

    public GenresController(IGenreService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAllGenres")]
    [AllowAnonymous]
    public ActionResult GetAll()
    {
        var list = _service.GetAllGenres().ToList();
        return Ok(list);
    }

    [HttpGet("{id:int}", Name = "GetGenreById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var dto = _service.GetGenreById(id);
        if (dto is null)
            return NotFound();
        return Ok(MapToResource(dto));
    }

    [HttpPost]
    [ApiTokenAuthorize]
    public ActionResult Create([FromBody] GenreInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _service.CreateNewGenre(input);
        var dto = _service.GetGenreById(id);
        var resource = dto is null ? null : MapToResource(dto);
        return CreatedAtAction(nameof(GetById), new { id }, resource);
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
