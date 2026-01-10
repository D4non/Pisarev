using System.Net;
using System.Text.Json;
using StackExchange.Redis;
using Project.Models.DTO;

namespace Project.Middleware;

public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RateLimitingMiddleware> _logger;
    private const int MaxRequestsPerMinute = 100;

    public RateLimitingMiddleware(
        RequestDelegate next,
        IConnectionMultiplexer redis,
        ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _redis = redis;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (!_redis.IsConnected)
            {
                _logger.LogWarning("Redis is not connected, skipping rate limiting");
            }
            else
            {
                var key = GetClientIdentifier(context);
                var db = _redis.GetDatabase();
                var current = await db.StringIncrementAsync($"ratelimit:{key}");

                if (current == 1)
                {
                    await db.KeyExpireAsync($"ratelimit:{key}", TimeSpan.FromMinutes(1));
                }

                if (current > MaxRequestsPerMinute)
                {
                    _logger.LogWarning("Rate limit exceeded for {Key}. Current: {Current}", key, current);
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = new ErrorResponseDto
                    {
                        Error = "TooManyRequests",
                        Message = "Rate limit exceeded. Maximum 100 requests per minute.",
                        TraceId = context.TraceIdentifier
                    };

                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var json = JsonSerializer.Serialize(response, options);
                    await context.Response.WriteAsync(json);
                    return;
                }

                context.Response.Headers["X-RateLimit-Limit"] = MaxRequestsPerMinute.ToString();
                context.Response.Headers["X-RateLimit-Remaining"] = (MaxRequestsPerMinute - current).ToString();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in rate limiting middleware, skipping rate limiting");
        }

        await _next(context);
    }

    private static string GetClientIdentifier(HttpContext context)
    {
        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}

