using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common.Models;
using Data;
using EducationPlatform.Extensions;
using EducationPlatform.Files;
using EducationPlatform.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Modules.Lessons.GetLessonVideo;

internal sealed class GetLessonVideoEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/lessons/{lessonId:guid}/video", Handle)
            .WithName("GetLessonVideo")
            .WithTags("Lessons")
            .WithSummary("Get protected lesson video")
            .RequireAuthorization();
    }

    private static async Task<Results<PhysicalFileHttpResult, NotFound, ForbidHttpResult>> Handle(
        [FromRoute] Guid lessonId,
        ClaimsPrincipal user,
        EducationDbContext dbContext,
        IFileStorage fileStorage,
        CancellationToken cancellationToken)
    {
        var lesson = await dbContext.Lessons
            .AsNoTracking()
            .Where(x => x.Id == lessonId)
            .Select(x => new
            {
                x.CourseId,
                x.VideoPath,
                x.VideoContentType,
                CourseCreatorId = x.Course == null ? (Guid?)null : x.Course.CreatorId,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (lesson is null ||
            lesson.CourseId is null ||
            lesson.CourseCreatorId is null ||
            lesson.VideoPath is null)
        {
            return TypedResults.NotFound();
        }

        var userId = user.GetRequiredUserId();
        var hasAccess = user.IsInRole("admin") ||
                        lesson.CourseCreatorId == userId ||
                        await dbContext.Subscriptions
                            .AsNoTracking()
                            .AnyAsync(
                                x => x.CourseId == lesson.CourseId.Value &&
                                     x.UserId == userId &&
                                     x.Status == SubscriptionStatus.Active,
                                cancellationToken);

        if (!hasAccess)
            return TypedResults.Forbid();

        var absolutePath = fileStorage.GetAbsolutePath(lesson.VideoPath);

        if (!File.Exists(absolutePath))
            return TypedResults.NotFound();

        return TypedResults.PhysicalFile(
            absolutePath,
            lesson.VideoContentType ?? "application/octet-stream",
            enableRangeProcessing: true);
    }
}
