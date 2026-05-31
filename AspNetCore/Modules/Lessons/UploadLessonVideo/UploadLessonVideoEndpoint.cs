using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Models;
using Common.Models;
using Data;
using EducationPlatform.Authentication;
using EducationPlatform.Extensions;
using EducationPlatform.Files;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Modules.Lessons.UploadLessonVideo;

internal sealed class UploadLessonVideoEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/lessons/{lessonId:guid}/video", Handle)
            .WithName("UploadLessonVideo")
            .WithTags("Lessons")
            .WithSummary("Upload lesson video")
            .DisableAntiforgery()
            .RequireAuthorization(AuthPolicies.CreatorOnly);
    }

    private static async Task<Results<Ok<Lesson>, BadRequest<string>, NotFound, ForbidHttpResult>> Handle(
        [FromRoute] Guid lessonId,
        IFormFile? file,
        ClaimsPrincipal user,
        EducationDbContext dbContext,
        IFileStorage fileStorage,
        CancellationToken cancellationToken)
    {
        if (!IsVideo(file))
            return TypedResults.BadRequest("Only video files are supported.");

        var lesson = await dbContext.Lessons
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == lessonId, cancellationToken);

        if (lesson is null)
            return TypedResults.NotFound();

        if (!user.IsInRole("admin") && lesson.Course?.CreatorId != user.GetRequiredUserId())
            return TypedResults.Forbid();

        var storedFile = await fileStorage.SaveAsync(file!, "lesson-videos", cancellationToken);
        fileStorage.Delete(lesson.VideoPath);

        lesson.VideoPath = storedFile.RelativePath;
        lesson.VideoContentType = storedFile.ContentType;
        lesson.MediaType = MediaType.Video;
        lesson.UpdateTimestamp = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(new Lesson
        {
            Id = lesson.Id,
            CourseId = lesson.CourseId,
            Name = lesson.Name,
            Summary = lesson.Summary,
            Description = lesson.Description,
            Text = lesson.Text,
            Status = lesson.Status,
            Duration = lesson.Duration,
            MediaType = lesson.MediaType,
            VideoUrl = $"/lessons/{lesson.Id}/video",
            CreateTimestamp = lesson.CreateTimestamp,
        });
    }

    private static bool IsVideo(IFormFile? file)
    {
        var extension = (Path.GetExtension(file?.FileName) ?? string.Empty).ToLowerInvariant();

        return file is not null &&
               file.Length > 0 &&
               file.Length <= 500 * 1024 * 1024 &&
               file.ContentType is not null &&
               file.ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase) &&
               extension is ".mp4" or ".webm" or ".mov";
    }
}
