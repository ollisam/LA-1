namespace AudioPool.Models.Entities;

public class Song
{
    // id of the song
    public int id { get; set; }

    // name of the song
    public string name { get; set; } = null!;

    // duration of the song
    public int duration { get; set; }

    // creation time of the song
    public DateTime DateCreated { get; set; }

    // modificaiton time of the song
    public DateTime? DateModified { get; set; }

    // modified by of the song e.g owner of the song
    public string? ModifiedBy { get; set; }
}

/*
Song
○ Id (int)
○ Name (string)
○ Duration (timespan)
○ DateCreated (code-generated)
○ DateModified (code-generated) NULL
○ ModifiedBy (code-generated) NULL
*/
