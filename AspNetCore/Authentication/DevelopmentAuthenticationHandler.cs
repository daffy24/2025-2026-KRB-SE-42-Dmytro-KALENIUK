using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EducationPlatform.Authentication;

internal sealed class DevelopmentAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "Development";

    private static readonly Guid DefaultUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var userId = TryGetUserIdFromHeader() ?? DefaultUserId;

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
        };

        claims.AddRange(GetRoles().Select(role => new Claim(ClaimTypes.Role, role)));

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private Guid? TryGetUserIdFromHeader()
    {
        if (!Request.Headers.TryGetValue("X-User-Id", out var values))
        {
            return null;
        }

        return Guid.TryParse(values.ToString(), out var userId)
            ? userId
            : null;
    }

    private IReadOnlyCollection<string> GetRoles()
    {
        if (!Request.Headers.TryGetValue("X-Roles", out var values))
        {
            return ["creator", "admin"];
        }

        return values
            .ToString()
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
