using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Requests;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Lessons.DeleteLesson;

internal sealed class DeleteLessonEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapDelete("/lessons/{lessonId:guid}", Handle)
            .WithName("DeleteLesson")
            .WithTags("Lessons")
            .WithSummary("Delete lesson")
            .RequireAuthorization();
    }

    private static async Task<Results<NoContent, NotFound>> Handle(
        [FromRoute] Guid lessonId,
        ClaimsPrincipal creator,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new DeleteLessonRequest(lessonId, creator.GetRequiredUserId());
        var deleted = await mediator.Send(request, cancellationToken);

        return deleted
            ? TypedResults.NoContent()
            : TypedResults.NotFound();
    }
}
