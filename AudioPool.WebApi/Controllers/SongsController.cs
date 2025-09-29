using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("api/songs")]
public class SongsController : ControllerBase
{
    private readonly ISongService _service;

    public SongsController(ISongService service)
    {
        _service = service;
    }

    // GET /songs?pageNumber=1&pageSize=10&containUnavailable=false
    [HttpGet(Name = "GetAllSongs")]
    public ActionResult GetAllSongs([FromQuery] bool containUnavailable = false) =>
        Ok(_service.GetAllSongs(containUnavailable));

    // GET /api/songs/{id}
    [HttpGet("{id:int}", Name = "GetSongById")]
    public ActionResult GetSongById(int id)
    {
        var details = _service.GetSongDetailsById(id);
        if (details is null) return NotFound();

        var response = new
        {
            details.id,
            details.name,
            duration = details.duration,
            album = details.album,
            trackNumberOnAlbum = details.trackNumberOnAlbum,
            _links = new
            {
                self = new { Href = $"/api/songs/{id}" },
                delete = new { Href = $"/api/songs/{id}" },
                edit = new { Href = $"/api/songs/{id}" },
                album = details.album is null ? null : new { Href = $"/api/albums/{details.album.id}" },
            }
        };
        return Ok(response);
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
        return CreatedAtAction(nameof(GetSongById), new { id }, dto);
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
}
