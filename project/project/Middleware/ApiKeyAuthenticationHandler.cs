using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Project.Repositories.Interfaces;

namespace Project.Middleware;

public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
{
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
{
    private readonly IApiKeyRepository _apiKeyRepository;

    public ApiKeyAuthenticationHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IApiKeyRepository apiKeyRepository)
        : base(options, logger, encoder)
    {
        _apiKeyRepository = apiKeyRepository;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-API-KEY", out var apiKeyHeaderValues))
        {
            return AuthenticateResult.NoResult();
        }

        var providedApiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(providedApiKey))
        {
            return AuthenticateResult.NoResult();
        }

        var apiKey = await _apiKeyRepository.GetByKeyAsync(providedApiKey);

        if (apiKey == null || !apiKey.IsActive || apiKey.ExpiresAt < DateTime.UtcNow)
        {
            return AuthenticateResult.Fail("Invalid or expired API key");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, apiKey.Id.ToString()),
            new Claim(ClaimTypes.Name, apiKey.Name),
            new Claim("ApiKey", "true")
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

