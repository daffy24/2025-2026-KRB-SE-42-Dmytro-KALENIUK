using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Lessons.GetLessons;

internal sealed class GetLessonsEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/courses/{courseId:guid}/lessons", Handle)
            .WithName("GetLessons")
            .WithTags("Lessons")
            .WithSummary("Get course lessons")
            .RequireAuthorization();
    }

    private static async Task<Results<Ok<IReadOnlyCollection<Lesson>>, NotFound, ForbidHttpResult>> Handle(
        [FromRoute] Guid courseId,
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetLessonsRequest(courseId, user.GetRequiredUserId(), user.IsInRole("admin"));
        var response = await mediator.Send(request, cancellationToken);

        return response.Status switch
        {
            LessonAccessStatus.Success => TypedResults.Ok(response.Lessons),
            LessonAccessStatus.Forbidden => TypedResults.Forbid(),
            _ => TypedResults.NotFound(),
        };
    }
}
