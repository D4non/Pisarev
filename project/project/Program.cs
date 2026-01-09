using Project.Data;
using Project.Middleware;
using Project.Repositories;
using Project.Repositories.Interfaces;
using Project.Services;
using Project.Services.Interfaces;
using Project.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Prometheus;
using Serilog;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Clubs Management API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "API Key authorization header. Example: \"X-API-KEY: {key}\"",
        Name = "X-API-KEY",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

var dbConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(dbConnectionString));

var redisConnection = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse(redisConnection ?? "localhost:6379");
    config.ConnectTimeout = 2000;
    config.SyncTimeout = 2000;
    config.AbortOnConnectFail = false;
    config.ConnectRetry = 0;
    return ConnectionMultiplexer.Connect(config);
});
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection ?? "localhost:6379";
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthentication().AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
    "ApiKey", options => { });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Manager", "Admin"));
    options.AddPolicy("UserOrAbove", policy => policy.RequireRole("User", "Manager", "Admin"));
});

builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();

builder.Services.AddScoped<IClubService, ClubService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateClubDtoValidator>();

builder.Services.AddHealthChecks()
    .AddCheck("postgresql", () =>
    {
        try
        {
            using var connection = new NpgsqlConnection(dbConnectionString);
            connection.Open();
            return HealthCheckResult.Healthy();
        }
        catch
        {
            return HealthCheckResult.Unhealthy();
        }
    })
    .AddRedis(redisConnection ?? "localhost:6379");

Log.Information("Building application...");
var app = builder.Build();
Log.Information("Application built successfully");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();
app.UseMiddleware<IdempotencyMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseHttpMetrics();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapMetrics("/metrics");

app.MapGet("/", () => Results.Json(new
{
    message = "Clubs Management API",
    version = "1.0",
    endpoints = new
    {
        swagger = "/swagger",
        health = "/health",
        metrics = "/metrics",
        api = "/api"
    }
}));

Log.Information("Starting application...");
app.Run();

