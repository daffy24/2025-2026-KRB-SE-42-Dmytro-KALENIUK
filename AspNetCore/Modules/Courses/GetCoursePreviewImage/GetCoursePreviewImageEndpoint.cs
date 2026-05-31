using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using EducationPlatform.Files;
using EducationPlatform.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Modules.Courses.GetCoursePreviewImage;

internal sealed class GetCoursePreviewImageEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/courses/{courseId:guid}/preview-image", Handle)
            .WithName("GetCoursePreviewImage")
            .WithTags("Courses")
            .WithSummary("Get course preview image");
    }

    private static async Task<Results<PhysicalFileHttpResult, NotFound>> Handle(
        [FromRoute] Guid courseId,
        EducationDbContext dbContext,
        IFileStorage fileStorage,
        CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses
            .AsNoTracking()
            .Where(x => x.Id == courseId)
            .Select(x => new
            {
                x.PreviewImagePath,
                x.PreviewImageContentType,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (course?.PreviewImagePath is null)
            return TypedResults.NotFound();

        var absolutePath = fileStorage.GetAbsolutePath(course.PreviewImagePath);

        if (!File.Exists(absolutePath))
            return TypedResults.NotFound();

        return TypedResults.PhysicalFile(
            absolutePath,
            course.PreviewImageContentType ?? "application/octet-stream");
    }
}
