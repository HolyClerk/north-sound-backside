using Microsoft.EntityFrameworkCore;
using NorthSound.Domain.Interfaces;
using NorthSound.Backend.Infrastructure;
using NorthSound.Backend.Infrastructure.Data;

namespace NorthSound.Backend.LibraryApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ConnectDatabase(builder);

        builder.Services
            .AddScoped<IAsyncSongRepository, AsyncSongRepository>()
            .AddScoped<ILibraryService, LibraryService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    public static void ConnectDatabase(WebApplicationBuilder builder)
    {
        string? connection = builder.Configuration.GetConnectionString("PgConnection");

        if (connection is null)
            throw new Exception("Wrong connection");

        builder.Services.AddDbContext<SongContext>(options => options.UseNpgsql(connection));
    }
}