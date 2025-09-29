using AudioPool.Repository.Data;
using AudioPool.Repository.Implementations;
using AudioPool.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// DbContext + repositories
builder.Services.AddDbContext<AudioPoolDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Ensure database file and schema exist

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AudioPoolDbContext>();
    db.Database.EnsureCreated(); // database exists
    var sqlPath = Path.Combine(app.Environment.ContentRootPath, "initial.sql");
    if (File.Exists(sqlPath))
    {
        var sql = await File.ReadAllTextAsync(sqlPath);
        await db.Database.ExecuteSqlRawAsync(sql);
        // Comment/remove after first run to avoid duplicate inserts
    }
}

app.Run();
