using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("albums")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumRepository _repository;

    public AlbumController(IAlbumRepository repository)
    {
        _repository = repository;
    }

    [HttpGet(Name = "GetAllAlbums")]
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

        var all = _repository.GetAllAlbums(containUnavailable).ToList();
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

    [HttpGet("{id:int}", Name = "GetAlbumById")]
    public ActionResult GetById(int id)
    {
        var dto = _repository.GetAlbumById(id);
        if (dto is null)
            return NotFound();
        return Ok(MapToResource(dto));
    }

    [HttpPost]
    public ActionResult Create([FromBody] AlbumInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _repository.CreateNewAlbum(input);
        var dto = _repository.GetAlbumById(id);
        var resource = dto is null ? null : MapToResource(dto);
        return CreatedAtAction(nameof(GetById), new { id }, resource);
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] AlbumInputModel input)
    {
        var exists = _repository.GetAlbumById(id) != null;
        if (!exists)
            return NotFound();
        _repository.UpdateAlbumById(id, input);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public ActionResult UpdatePartially(int id, [FromBody] AlbumPartialInputModel input)
    {
        var exists = _repository.GetAlbumById(id) != null;
        if (!exists)
            return NotFound();
        _repository.UpdateAlbumPartiallyById(id, input);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var exists = _repository.GetAlbumById(id) != null;
        if (!exists)
            return NotFound();
        _repository.DeleteAlbumById(id);
        return NoContent();
    }

    private object MapToResource(AlbumDto dto)
    {
        return new
        {
            dto.id,
            dto.name,
            dto.releaseDate,
            dto.coverImageUrl,
            _links = new { self = new LinkRepresentation { Href = BuildAlbumUrl(dto.id) } },
        };
    }

    private string BuildAlbumUrl(int id) => Url.Link("GetAlbumById", new { id })!;

    private string BuildPageUrl(int pageNumber, int pageSize, bool containUnavailable) =>
        Url.Link(
            "GetAllAlbums",
            new
            {
                pageNumber,
                pageSize,
                containUnavailable,
            }
        )!;
}
