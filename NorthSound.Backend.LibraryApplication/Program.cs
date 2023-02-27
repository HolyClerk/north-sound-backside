using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Services;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Infrastructure;

namespace NorthSound.Backend.LibraryApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication("OAuth")
            .AddJwtBearer("OAuth", config =>
            {
                var tokenGen = new JwtTokenGenerator();

                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = JwtTokenGenerator.Issuer,
                    ValidAudience = JwtTokenGenerator.Audience,
                    IssuerSigningKey = tokenGen.SecurityKey
                };
            });

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        ConnectDatabase(builder);

        builder.Services
            .AddTransient<IStorageGenerator, StorageGenerator>()    // Класс, необходимый для создания путей
            .AddTransient<ITokenHandler, JwtTokenGenerator>()       // Сервис работы с токенами
            .AddScoped<IAsyncSongRepository, AsyncSongRepository>() // Репозиторий музыки
            .AddScoped<ILibraryService, LibraryService>()           // Сервис, работающий с репо музыки
            .AddScoped<IUserRepository, UserRepository>()           // Репо пользователей
            .AddScoped<IUserService, UserService>();                // Сервис, работающий с авторизацией пользователей

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();

        app.Run();
    }

    public static void ConnectDatabase(WebApplicationBuilder builder)
    {
        string? connection = builder.Configuration.GetConnectionString("PgConnection");

        if (connection is null)
            throw new Exception("Wrong connection");

        builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
    }
}