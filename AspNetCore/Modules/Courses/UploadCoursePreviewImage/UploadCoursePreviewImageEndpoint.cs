using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Courses.Models;
using Data;
using EducationPlatform.Authentication;
using EducationPlatform.Extensions;
using EducationPlatform.Files;
using EducationPlatform.Modules;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Modules.Courses.UploadCoursePreviewImage;

internal sealed class UploadCoursePreviewImageEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/courses/{courseId:guid}/preview-image", Handle)
            .WithName("UploadCoursePreviewImage")
            .WithTags("Courses")
            .WithSummary("Upload course preview image")
            .DisableAntiforgery()
            .RequireAuthorization(AuthPolicies.CreatorOnly);
    }

    private static async Task<Results<Ok<Course>, BadRequest<string>, NotFound, ForbidHttpResult>> Handle(
        [FromRoute] Guid courseId,
        [FromForm] IFormFile? file,
        ClaimsPrincipal user,
        EducationDbContext dbContext,
        IFileStorage fileStorage,
        CancellationToken cancellationToken)
    {
        if (!IsImage(file))
            return TypedResults.BadRequest("Only image files are supported.");

        var course = await dbContext.Courses
            .FirstOrDefaultAsync(x => x.Id == courseId, cancellationToken);

        if (course is null)
            return TypedResults.NotFound();

        if (!user.IsInRole("admin") && course.CreatorId != user.GetRequiredUserId())
            return TypedResults.Forbid();

        var storedFile = await fileStorage.SaveAsync(file!, "course-previews", cancellationToken);
        fileStorage.Delete(course.PreviewImagePath);

        course.PreviewImagePath = storedFile.RelativePath;
        course.PreviewImageContentType = storedFile.ContentType;
        course.UpdateTimestamp = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return TypedResults.Ok(new Course
        {
            Id = course.Id,
            CreatorId = course.CreatorId,
            Name = course.Name,
            Summary = course.Summary,
            Description = course.Description,
            PreviewImageUrl = $"/courses/{course.Id}/preview-image",
            Language = course.Language,
            Price = course.Price,
            Tags = course.Tags,
            Status = course.Status,
            UpdateTimestamp = course.UpdateTimestamp,
        });
    }

    private static bool IsImage(IFormFile? file)
    {
        return file is not null &&
               file.Length > 0 &&
               file.Length <= 10 * 1024 * 1024 &&
               file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase);
    }
}
