using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using EducationPlatform.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Authentication;

internal sealed class CreatorAccessRequirement : IAuthorizationRequirement;

internal sealed class CreatorAccessRequirementHandler(EducationDbContext dbContext)
    : AuthorizationHandler<CreatorAccessRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CreatorAccessRequirement requirement)
    {
        if (context.User.IsInRole("admin") || context.User.IsInRole("creator"))
        {
            context.Succeed(requirement);
            return;
        }

        var userId = GetUserId(context.User);
        if (userId is null)
            return;

        var hasCreatorAccess = await dbContext.Creators
            .AnyAsync(x => x.Id == userId.Value);

        if (hasCreatorAccess)
            context.Succeed(requirement);
    }

    private static Guid? GetUserId(ClaimsPrincipal user)
    {
        try
        {
            return user.GetRequiredUserId();
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }
}
