using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Services;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;

void ConnectDatabase(WebApplicationBuilder builder)
{
    string? connection = builder.Configuration.GetConnectionString("PgConnection");

    if (connection is null)
        throw new Exception("Wrong connection");

    builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
}

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", config =>
    {
        var tokenGen = new JwtTokenGenerator(builder.Configuration);
        var key = tokenGen.GetSymmetricSecurityKey();

        config.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Query.ContainsKey("t"))
                    context.Token = context.Request.Query["t"];

                return Task.CompletedTask;
            }
        };

        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidIssuer = JwtTokenGenerator.Issuer,
            ValidAudience = JwtTokenGenerator.Audience,
            IssuerSigningKey = key,
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConnectDatabase(builder);

builder.Services
    .AddTransient<IStorageGenerator, StorageGenerator>()        // Класс, необходимый для создания путей
    .AddTransient<ITokenHandler, JwtTokenGenerator>()           // Сервис работы с JWT токенами
    .AddScoped<IAsyncSongRepository, AsyncSongRepository>()     // Репозиторий музыки
    .AddScoped<ILibraryService, LibraryService>()               // Сервис, работающий с репо музыки
    .AddScoped<IAccountService, AccountService>();    // Сервис, работающий с авторизацией пользователей

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();