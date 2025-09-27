using Microsoft.AspNetCore.Mvc;

namespace AudioPool.WebApi.Controllers;

[ApiController]
[Route("[controller]")]

public class SongsController : ControllerBase
{
    // http://localhost:5124/songs
    [HttpGet("")]
    public ActionResult GetAllSongs()
    {
        return Ok();
    }

    // http://localhost:5124/songs/{id}
    [HttpGet("{id:int}")]
    public ActionResult GetSongById(int id)
    {
        return Ok(id);
    }

    // http://localhost:5124/songs/
    [HttpPost("")]
    public ActionResult CreateSong()
    {
        return NoContent();
    }


    // http://localhost:5124/songs/{id}
    [HttpPut("{id:int}")]
    public ActionResult UpdateSongById(int id)
    {
        return NoContent();
    }

    // http://localhost:5124/songs/{id}
    [HttpDelete("{id:int}")]
    public ActionResult DeleteSongById(int id)
    {
        return NoContent();
    }
}
