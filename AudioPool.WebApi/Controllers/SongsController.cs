using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SongsController : ControllerBase
{
    private readonly ISongService _service;

    public SongsController(ISongService service)
    {
        _service = service;
    }

    // GET /songs?pageNumber=1&pageSize=10&containUnavailable=false
    [HttpGet(Name = "GetAllSongs")]
    public ActionResult GetAllSongs(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool containUnavailable = false)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var all = _service.GetAllSongs(containUnavailable).ToList();
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
                self = new LinkRepresentation { Href = BuildPageUrl(pageNumber, pageSize, containUnavailable) },
                first = new LinkRepresentation { Href = BuildPageUrl(1, pageSize, containUnavailable) },
                prev = new LinkRepresentation { Href = BuildPageUrl(Math.Max(1, pageNumber - 1), pageSize, containUnavailable) },
                next = new LinkRepresentation { Href = BuildPageUrl(Math.Min(Math.Max(1, envelope.MaxPages), pageNumber + 1), pageSize, containUnavailable) },
                last = new LinkRepresentation { Href = BuildPageUrl(Math.Max(1, envelope.MaxPages), pageSize, containUnavailable) }
            }
        };

        return Ok(result);
    }

    // GET /songs/{id}
    [HttpGet("{id:int}", Name = "GetSongById")]
    public ActionResult GetSongById(int id)
    {
        var dto = _service.GetSongById(id);
        if (dto is null)
        {
            return NotFound();
        }
        return Ok(MapToResource(dto));
    }

    // POST /songs
    [HttpPost]
    [ApiTokenAuthorize]
    public ActionResult CreateSong([FromBody] SongInputModel input)
    {
        if (input is null)
        {
            return BadRequest();
        }

        var id = _service.CreateNewSong(input);
        var dto = _service.GetSongById(id);
        var resource = dto is null ? null : MapToResource(dto);
        return CreatedAtAction(nameof(GetSongById), new { id }, resource);
    }

    // PUT /songs/{id}
    [HttpPut("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult UpdateSongById(int id, [FromBody] SongInputModel input)
    {
        var exists = _service.GetSongById(id) != null;
        if (!exists)
        {
            return NotFound();
        }

        _service.UpdateSongById(id, input);
        return NoContent();
    }

    // PATCH /songs/{id}
    [HttpPatch("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult UpdateSongPartiallyById(int id, [FromBody] SongPartialInputModel input)
    {
        var exists = _service.GetSongById(id) != null;
        if (!exists)
        {
            return NotFound();
        }

        _service.UpdateSongPartiallyById(id, input);
        return NoContent();
    }

    // DELETE /songs/{id}
    [HttpDelete("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult DeleteSongById(int id)
    {
        var exists = _service.GetSongById(id) != null;
        if (!exists)
        {
            return NotFound();
        }

        _service.DeleteSongById(id);
        return NoContent();
    }

    private object MapToResource(SongDto dto)
    {
        return new
        {
            dto.id,
            dto.name,
            dto.duration,
            dto.albumId,
            _links = new
            {
                self = new LinkRepresentation { Href = BuildSongUrl(dto.id) }
            }
        };
    }

    private string BuildSongUrl(int id) => Url.Link("GetSongById", new { id })!;

    private string BuildPageUrl(int pageNumber, int pageSize, bool containUnavailable)
    {
        return Url.Link("GetAllSongs", new { pageNumber, pageSize, containUnavailable })!;
    }
}
