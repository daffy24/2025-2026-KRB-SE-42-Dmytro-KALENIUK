using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Courses.Requests;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Courses.DeleteCourse;

internal sealed class DeleteCourseEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapDelete("/courses/{courseId:guid}", Handle)
            .WithName("DeleteCourse")
            .WithTags("Courses")
            .WithSummary("Delete course")
            .RequireAuthorization();
    }

    private static async Task<Results<NoContent, NotFound>> Handle(
        [FromRoute] Guid courseId,
        ClaimsPrincipal creator,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteCourseRequest(courseId, creator.GetRequiredUserId());
        var deleted = await mediator.Send(request, cancellationToken);

        return deleted
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
