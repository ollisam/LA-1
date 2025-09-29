using AudioPool.Models.InputModels;
using AudioPool.Services.Interfaces;
using AudioPool.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("api/artists")]
public class ArtistController : ControllerBase
{
    private readonly IArtistService _service;
    private readonly IGenreService _genreService;

    public ArtistController(IArtistService service, IGenreService genreService)
    {
        _service = service;
        _genreService = genreService;
    }

    [HttpGet(Name = "GetAllArtists")]
    [AllowAnonymous]
    public ActionResult GetAll([FromQuery] int pageSize = 25) => Ok(_service.GetArtists(pageSize));

    [HttpGet("{id:int}", Name = "GetArtistById")]
    [AllowAnonymous]
    public ActionResult GetById(int id)
    {
        var details = _service.GetArtistDetailsById(id);
        if (details is null) return NotFound();

        var response = new
        {
            details.id,
            details.name,
            details.bio,
            details.coverImageUrl,
            details.dateOfStart,
            albums = details.albums.Select(a => new
            {
                a.id,
                a.name,
                a.releaseDate,
                a.coverImageUrl,
                a.description,
                _links = new { }
            }),
            genres = details.genres.Select(g => new
            {
                g.id,
                g.name,
                _links = new { }
            }),
            _links = new
            {
                self = new { href = $"/api/artists/{id}" },
                edit = new { href = $"/api/artists/{id}" },
                delete = new { href = $"/api/artists/{id}" },
                albums = new { href = $"/api/artists/{id}/albums" },
                genres = details.genres.Select(g => new { href = $"/api/genres/{g.id}" })
            }
        };

        return Ok(response);
    }

    // GET /api/artists/{id}/albums
    [HttpGet("{id:int}/albums")]
    [AllowAnonymous]
    public ActionResult GetArtistAlbums(int id, [FromQuery] int pageSize = 25)
    {
        if (_service.GetArtistById(id) is null)
            return NotFound();

        var albums = _service.GetAlbumsForArtist(id, pageSize).ToList();
        var shaped = albums.Select(a => new
        {
            a.id,
            a.name,
            a.releaseDate,
            a.coverImageUrl,
            a.description,
            _links = new
            {
                self = new { href = $"/api/albums/{a.id}" },
                delete = new { href = $"/api/albums/{a.id}" },
                songs = new { href = $"/api/albums/{a.id}/songs" },
                artists = new[] { new { href = $"/api/artists/{id}" } }
            }
        });

        return Ok(shaped);
    }

    // PATCH /api/artists/{artistId}/genres/{genreId}
    [HttpPatch("{artistId:int}/genres/{genreId:int}")]
    [ApiTokenAuthorize]
    public ActionResult LinkArtistToGenre(int artistId, int genreId)
    {
        if (_service.GetArtistById(artistId) is null) return NotFound();
        if (_genreService.GetGenreById(genreId) is null) return NotFound();
        _service.LinkArtistToGenre(artistId, genreId);
        return NoContent();
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
