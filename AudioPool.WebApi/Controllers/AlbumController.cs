using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("/albums")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumService _service;

    public AlbumController(IAlbumService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAllAlbums")]
    [AllowAnonymous]
    public ActionResult GetAll([FromQuery] int pageSize = 25) => Ok(_service.GetAlbums(pageSize));

    [HttpGet("{id:int}", Name = "GetAlbumById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var dto = _service.GetAlbumById(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    // GET /api/albums/{id}/songs
    [HttpGet("{id:int}/songs")]
    [AllowAnonymous]
    public ActionResult GetSongsOnAlbum(int id, [FromQuery] int pageSize = 25)
    {
        if (_service.GetAlbumById(id) is null)
            return NotFound();
        return Ok(_service.GetSongsOnAlbum(id, pageSize));
    }

    [HttpPost]
    [ApiTokenAuthorize]
    public ActionResult Create([FromBody] AlbumInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _service.CreateNewAlbum(input);
        var dto = _service.GetAlbumById(id);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult Update(int id, [FromBody] AlbumInputModel input)
    {
        var exists = _service.GetAlbumById(id) != null;
        if (!exists)
            return NotFound();
        _service.UpdateAlbumById(id, input);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult UpdatePartially(int id, [FromBody] AlbumPartialInputModel input)
    {
        var exists = _service.GetAlbumById(id) != null;
        if (!exists)
            return NotFound();
        _service.UpdateAlbumPartiallyById(id, input);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult Delete(int id)
    {
        var exists = _service.GetAlbumById(id) != null;
        if (!exists)
            return NotFound();
        _service.DeleteAlbumById(id);
        return NoContent();
    }
}
