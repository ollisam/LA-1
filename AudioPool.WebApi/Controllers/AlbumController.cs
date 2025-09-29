using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/albums")]
public class AlbumController : ControllerBase
{
    private readonly IAlbumRepository _repository;
    private readonly ISongRepository _songs;

    public AlbumController(IAlbumRepository repository, ISongRepository songs)
    {
        _repository = repository;
        _songs = songs;
    }

    [HttpGet(Name = "GetAllAlbums")]
    public ActionResult GetAll([FromQuery] int pageSize = 25)
    {
        if (pageSize < 1) pageSize = 25;
        var list = _repository.GetAllAlbums(false).Take(pageSize).ToList();
        return Ok(list);
    }

    [HttpGet("{id:int}", Name = "GetAlbumById")]
    public ActionResult GetById(int id)
    {
        var dto = _repository.GetAlbumById(id);
        if (dto is null)
            return NotFound();
        return Ok(MapToResource(dto));
    }

    // GET /api/albums/{id}/songs
    [HttpGet("{id:int}/songs")]
    public ActionResult GetSongsOnAlbum(int id, [FromQuery] int pageSize = 25)
    {
        if (_repository.GetAlbumById(id) is null) return NotFound();
        if (pageSize < 1) pageSize = 25;
        var songs = _songs.GetSongsByAlbumId(id).Take(pageSize).ToList();
        return Ok(songs);
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

    private string BuildPageUrl(int pageNumber, int pageSize, bool containUnavailable) => "";
}
