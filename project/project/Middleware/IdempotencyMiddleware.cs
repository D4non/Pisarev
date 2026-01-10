using System.Text;
using System.Text.Json;
using StackExchange.Redis;

namespace Project.Middleware;

public class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<IdempotencyMiddleware> _logger;

    public IdempotencyMiddleware(
        RequestDelegate next,
        IConnectionMultiplexer redis,
        ILogger<IdempotencyMiddleware> logger)
    {
        _next = next;
        _redis = redis;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post)
        {
            if (context.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey))
            {
                var key = idempotencyKey.ToString();
                if (!string.IsNullOrWhiteSpace(key))
                {
                    try
                    {
                        if (!_redis.IsConnected)
                        {
                            _logger.LogWarning("Redis is not connected, skipping idempotency check");
                            await _next(context);
                            return;
                        }

                        var db = _redis.GetDatabase();
                        var existingResponse = await db.StringGetAsync($"idempotency:{key}");

                        if (existingResponse.HasValue)
                        {
                            _logger.LogInformation("Idempotency key {Key} found, returning cached response", key);
                            var cachedResponse = JsonSerializer.Deserialize<CachedResponse>(existingResponse!);
                            if (cachedResponse != null)
                            {
                                context.Response.StatusCode = cachedResponse.StatusCode;
                                context.Response.ContentType = cachedResponse.ContentType;
                                await context.Response.WriteAsync(cachedResponse.Body);
                                return;
                            }
                        }

                        var originalBodyStream = context.Response.Body;
                        using var responseBody = new MemoryStream();
                        context.Response.Body = responseBody;

                        await _next(context);

                        responseBody.Seek(0, SeekOrigin.Begin);
                        var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
                        responseBody.Seek(0, SeekOrigin.Begin);

                        if (context.Response.StatusCode >= 200 && context.Response.StatusCode < 300)
                        {
                            var cachedResponse = new CachedResponse
                            {
                                StatusCode = context.Response.StatusCode,
                                ContentType = context.Response.ContentType ?? "application/json",
                                Body = responseBodyText
                            };

                            var serialized = JsonSerializer.Serialize(cachedResponse);
                            await db.StringSetAsync($"idempotency:{key}", serialized, TimeSpan.FromHours(24));
                        }

                        await responseBody.CopyToAsync(originalBodyStream);
                        context.Response.Body = originalBodyStream;
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in idempotency middleware, skipping idempotency check");
                    }
                }
            }
        }

        await _next(context);
    }

    private class CachedResponse
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}

