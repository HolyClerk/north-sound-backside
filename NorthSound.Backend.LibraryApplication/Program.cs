using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NorthSound.Backend.DAL;
using NorthSound.Backend.Services;
using NorthSound.Backend.Services.Abstractions;
using NorthSound.Backend.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using NorthSound.Backend.LibraryApplication.Hubs;
using NorthSound.Backend.Services.Other;

void ConnectDatabase(WebApplicationBuilder builder)
{
    string? connection = builder.Configuration.GetConnectionString("PgConnection");

    if (connection is null)
        throw new Exception("Wrong connection");

    builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
}

var builder = WebApplication.CreateBuilder(args);

// JWT
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

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

ConnectDatabase(builder);

// SERVICES
builder.Services
    .AddTransient<ILocator, Locator>()                          // �����, ����������� ��� �������� �����
    .AddTransient<ITokenHandler, JwtTokenGenerator>()           // ������ ������ � JWT ��������
    .AddScoped<ILibraryService, LibraryService>()               // ������, ���������� � ���� ������
    .AddScoped<IAccountService, AccountService>()               // ������, ���������� � ������������ �������������
    .AddScoped<IDialogueService, DialogueService>()
    .AddScoped<IChatService, ChatService>()
    .AddSingleton<ISessions, Sessions>();

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
app.MapHub<ChatHub>("/chat");

app.Run();