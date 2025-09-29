using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("genres")]
public class GenresController : ControllerBase
{
    private readonly IGenreRepository _repository;

    public GenresController(IGenreRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(Name = "GetAllGenres")]
    public ActionResult GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool containUnavailable = false
    )
    {
        if (pageNumber < 1)
            pageNumber = 1;
        if (pageSize < 1)
            pageSize = 10;

        var all = _repository.GetAllGenres(containUnavailable).ToList();
        var items = all.Select(MapToResource).ToList();
        var envelope = new Envelope<object>(pageNumber, pageSize, items);
        var result = new
        {
            envelope.PageNumber,
            envelope.PageSize,
            envelope.MaxPages,
            Items = envelope.Items,
            _links = new
            {
                self = new LinkRepresentation
                {
                    Href = BuildPageUrl(pageNumber, pageSize, containUnavailable),
                },
                first = new LinkRepresentation
                {
                    Href = BuildPageUrl(1, pageSize, containUnavailable),
                },
                prev = new LinkRepresentation
                {
                    Href = BuildPageUrl(Math.Max(1, pageNumber - 1), pageSize, containUnavailable),
                },
                next = new LinkRepresentation
                {
                    Href = BuildPageUrl(
                        Math.Min(Math.Max(1, envelope.MaxPages), pageNumber + 1),
                        pageSize,
                        containUnavailable
                    ),
                },
                last = new LinkRepresentation
                {
                    Href = BuildPageUrl(
                        Math.Max(1, envelope.MaxPages),
                        pageSize,
                        containUnavailable
                    ),
                },
            },
        };
        return Ok(result);
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

    private string BuildPageUrl(int pageNumber, int pageSize, bool containUnavailable) =>
        Url.Link(
            "GetAllGenres",
            new
            {
                pageNumber,
                pageSize,
                containUnavailable,
            }
        )!;
}
