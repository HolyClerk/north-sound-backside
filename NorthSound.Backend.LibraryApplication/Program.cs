using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthSound.Backend.DAL.Abstractions;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Services;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.AspNetCore.Authentication.OAuth;

void ConnectDatabase(WebApplicationBuilder builder)
{
    string? connection = builder.Configuration.GetConnectionString("PgConnection");

    if (connection is null)
        throw new Exception("Wrong connection");

    builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidIssuer = JwtTokenGenerator.Issuer,
            ValidAudience = JwtTokenGenerator.Audience,
            IssuerSigningKey = new JwtTokenGenerator(builder.Configuration).GetSymmetricSecurityKey(),
        };
    }
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConnectDatabase(builder);

builder.Services
    .AddTransient<IStorageGenerator, StorageGenerator>()        // �����, ����������� ��� �������� �����
    .AddTransient<ITokenHandler, JwtTokenGenerator>()           // ������ ������ � JWT ��������
    .AddScoped<IAsyncSongRepository, AsyncSongRepository>()     // ����������� ������
    .AddScoped<ILibraryService, LibraryService>()               // ������, ���������� � ���� ������
    .AddScoped<IAccountService, AccountService>();              // ������, ���������� � ������������ �������������

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