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
    public ActionResult GetAll([FromQuery] int pageSize = 25) => Ok(_service.GetArtists(pageSize));

    [HttpGet("{id:int}", Name = "GetArtistById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var dto = _service.GetArtistById(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    // GET /api/artists/{id}/albums
    [HttpGet("{id:int}/albums")]
    [AllowAnonymous]
    public ActionResult GetArtistAlbums(int id, [FromQuery] int pageSize = 25)
    {
        if (_service.GetArtistById(id) is null)
            return NotFound();
        return Ok(_service.GetAlbumsForArtist(id, pageSize));
    }

    [HttpPost]
    [ApiTokenAuthorize]
    public ActionResult Create([FromBody] ArtistInputModel input)
    {
        if (input is null)
            return BadRequest();
        var id = _service.CreateNewArtist(input);
        var dto = _service.GetArtistById(id);
        return CreatedAtAction(nameof(GetById), new { id }, dto);
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
}
