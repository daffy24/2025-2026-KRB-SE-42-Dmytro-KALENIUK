using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace EducationPlatform.Authentication;

internal static class AuthenticationExtensions
{
    public static IServiceCollection AddEducationPlatformAuthentication(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        if (environment.IsDevelopment() && configuration.GetValue<bool>("Authentication:UseDevelopmentAuth"))
        {
            services
                .AddAuthentication(DevelopmentAuthenticationHandler.SchemeName)
                .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, DevelopmentAuthenticationHandler>(
                    DevelopmentAuthenticationHandler.SchemeName,
                    _ => { });

            return services;
        }

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var audience = configuration["Authentication:Audience"];
                var issuer = configuration["Authentication:Issuer"];
                var metadataAddress = configuration["Authentication:MetadataAddress"];

                options.Authority = configuration["Authentication:Authority"];
                options.Audience = audience;
                options.RequireHttpsMetadata = configuration.GetValue("Authentication:RequireHttpsMetadata", true);
                options.MapInboundClaims = false;

                if (!string.IsNullOrWhiteSpace(metadataAddress))
                {
                    options.MetadataAddress = metadataAddress;
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "preferred_username",
                    RoleClaimType = ClaimTypes.Role,
                    ValidAudience = audience,
                    ValidIssuer = issuer,
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        AddKeycloakRoleClaims(context.Principal);
                        return Task.CompletedTask;
                    },
                };
            });

        return services;
    }

    public static IServiceCollection AddEducationPlatformAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicies.CreatorOnly, policy =>
                policy.RequireRole("creator", "admin"));

            options.AddPolicy(AuthPolicies.AdminOnly, policy =>
                policy.RequireRole("admin"));

            options.AddPolicy(AuthPolicies.StudentOrCreatorOnly, policy =>
                policy.RequireRole("student", "creator"));
        });

        return services;
    }

    private static void AddKeycloakRoleClaims(ClaimsPrincipal? principal)
    {
        if (principal?.Identity is not ClaimsIdentity identity)
        {
            return;
        }

        foreach (var role in GetRealmRoles(principal).Concat(GetClientRoles(principal)))
        {
            if (!principal.IsInRole(role))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
        }
    }

    private static IEnumerable<string> GetRealmRoles(ClaimsPrincipal principal)
    {
        var realmAccess = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess))
        {
            return [];
        }

        using var document = JsonDocument.Parse(realmAccess);
        return ReadRoles(document.RootElement).ToArray();
    }

    private static IEnumerable<string> GetClientRoles(ClaimsPrincipal principal)
    {
        var resourceAccess = principal.FindFirst("resource_access")?.Value;
        if (string.IsNullOrWhiteSpace(resourceAccess))
        {
            return [];
        }

        using var document = JsonDocument.Parse(resourceAccess);
        var roles = new List<string>();

        foreach (var client in document.RootElement.EnumerateObject())
        {
            roles.AddRange(ReadRoles(client.Value));
        }

        return roles;
    }

    private static IEnumerable<string> ReadRoles(JsonElement element)
    {
        if (!element.TryGetProperty("roles", out var rolesElement) ||
            rolesElement.ValueKind is not JsonValueKind.Array)
        {
            return [];
        }

        return rolesElement
            .EnumerateArray()
            .Where(role => role.ValueKind is JsonValueKind.String)
            .Select(role => role.GetString())
            .Where(role => !string.IsNullOrWhiteSpace(role))!;
    }
}
