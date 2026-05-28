using System;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EducationPlatform.Extensions;

public static class ClaimsPrincipalExtensions
{
    extension(ClaimsPrincipal user)
    {
        private Guid? GetUserId()
        {
            string? value =
                user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
                user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Guid.TryParse(value, out var id) ? id : null;
        }

        public Guid GetRequiredUserId()
        {
            var userId = user.GetUserId();
            return userId ?? throw new UnauthorizedAccessException("User identifier claim is missing or invalid.");
        }
    }
}
