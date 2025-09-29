using AudioPool.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AudioPool.Repository.Data;

public class AudioPoolDbContext(DbContextOptions<AudioPoolDbContext> options) : DbContext(options)
{
    public DbSet<Song> Songs => Set<Song>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Song>().ToTable("Songs");
        modelBuilder.Entity<Album>().ToTable("Albums");
        modelBuilder.Entity<Artist>().ToTable("Artists");
        modelBuilder.Entity<Genre>().ToTable("Genres");

        modelBuilder
            .Entity<Song>()
            .HasOne(s => s.Album)
            .WithMany(a => a.Songs)
            .HasForeignKey(s => s.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Album>()
            .HasMany(a => a.Artists)
            .WithMany(ar => ar.Albums)
            .UsingEntity(j => j.ToTable("AlbumArtist"));

        modelBuilder
            .Entity<Artist>()
            .HasMany(a => a.Genres)
            .WithMany(g => g.Artists)
            .UsingEntity(j => j.ToTable("ArtistGenre"));
    }
}
