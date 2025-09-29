using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("/songs")]
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

    // GET /songs/{id}
    [HttpGet("{id:int}", Name = "GetSongById")]
    public ActionResult GetSongById(int id)
    {
        var dto = _service.GetSongById(id);
        if (dto is null)
            return NotFound();
        return Ok(dto);
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
