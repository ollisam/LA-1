using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/artists")]
public class ArtistController : ControllerBase
{
    private readonly IArtistRepository _repository;
    private readonly IAlbumRepository _albums;

    public ArtistController(IArtistRepository repository, IAlbumRepository albums)
    {
        _repository = repository;
        _albums = albums;
    }

    [HttpGet(Name = "GetAllArtists")]
    public ActionResult GetAll([FromQuery] int pageSize = 25)
    {
        if (pageSize < 1) pageSize = 25;
        var list = _repository.GetAllArtists(false).Take(pageSize).ToList();
        return Ok(list);
    }

    [HttpGet("{id:int}", Name = "GetArtistById")]
    public ActionResult GetById(int id)
    {
        var dto = _repository.GetArtistById(id);
        if (dto is null)
            return NotFound();
        return Ok(MapToResource(dto));
    }

    // GET /api/artists/{id}/albums
    [HttpGet("{id:int}/albums")]
    public ActionResult GetArtistAlbums(int id, [FromQuery] int pageSize = 25)
    {
        if (_repository.GetArtistById(id) is null) return NotFound();
        if (pageSize < 1) pageSize = 25;
        var albums = _albums.GetAlbumsByArtistId(id).Take(pageSize).ToList();
        return Ok(albums);
    }

    [HttpPost]
    public ActionResult Create([FromBody] ArtistInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _repository.CreateNewArtist(input);
        var dto = _repository.GetArtistById(id);
        var resource = dto is null ? null : MapToResource(dto);
        return CreatedAtAction(nameof(GetById), new { id }, resource);
    }

    [HttpPut("{id:int}")]
    public ActionResult Update(int id, [FromBody] ArtistInputModel input)
    {
        var exists = _repository.GetArtistById(id) != null;
        if (!exists)
            return NotFound();
        _repository.UpdateArtistById(id, input);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    public ActionResult UpdatePartially(int id, [FromBody] ArtistPartialInputModel input)
    {
        var exists = _repository.GetArtistById(id) != null;
        if (!exists)
            return NotFound();
        _repository.UpdateArtistPartiallyById(id, input);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var exists = _repository.GetArtistById(id) != null;
        if (!exists)
            return NotFound();
        _repository.DeleteArtistById(id);
        return NoContent();
    }

    private object MapToResource(ArtistDto dto)
    {
        return new
        {
            dto.id,
            dto.name,
            dto.coverImageUrl,
            _links = new { self = new LinkRepresentation { Href = BuildArtistUrl(dto.id) } },
        };
    }

    private string BuildArtistUrl(int id) => Url.Link("GetArtistById", new { id })!;

    private string BuildPageUrl(int pageNumber, int pageSize, bool containUnavailable) => "";
}
