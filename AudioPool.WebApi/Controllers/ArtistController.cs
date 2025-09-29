using AudioPool.Models;
using AudioPool.Models.Dtos;
using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("/artists")]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _service;

    public ArtistController(IArtistService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetAllArtists")]
    [AllowAnonymous]
    public ActionResult GetAll([FromQuery] int pageSize = 25)
    {
        if (pageSize < 1) pageSize = 25;
        var list = _service.GetArtists(pageSize).ToList();
        return Ok(list);
    }

    [HttpGet("{id:int}", Name = "GetArtistById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var dto = _service.GetArtistById(id);
        if (dto is null)
            return NotFound();
        return Ok(MapToResource(dto));
    }

    // GET /api/artists/{id}/albums
    [HttpGet("{id:int}/albums")]
    [AllowAnonymous]
    public ActionResult GetArtistAlbums(int id, [FromQuery] int pageSize = 25)
    {
        if (_service.GetArtistById(id) is null) return NotFound();
        if (pageSize < 1) pageSize = 25;
        var albums = _service.GetAlbumsForArtist(id, pageSize).ToList();
        return Ok(albums);
    }

    [HttpPost]
    [ApiTokenAuthorize]
    public ActionResult Create([FromBody] ArtistInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _service.CreateNewArtist(input);
        var dto = _service.GetArtistById(id);
        var resource = dto is null ? null : MapToResource(dto);
        return CreatedAtAction(nameof(GetById), new { id }, resource);
    }

    [HttpPut("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult Update(int id, [FromBody] ArtistInputModel input)
    {
        var exists = _service.GetArtistById(id) != null;
        if (!exists)
            return NotFound();
        _service.UpdateArtistById(id, input);
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult UpdatePartially(int id, [FromBody] ArtistPartialInputModel input)
    {
        var exists = _service.GetArtistById(id) != null;
        if (!exists)
            return NotFound();
        _service.UpdateArtistPartiallyById(id, input);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ApiTokenAuthorize]
    public ActionResult Delete(int id)
    {
        var exists = _service.GetArtistById(id) != null;
        if (!exists)
            return NotFound();
        _service.DeleteArtistById(id);
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
