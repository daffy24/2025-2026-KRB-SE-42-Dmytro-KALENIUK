using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using EducationPlatform.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Lessons.GetLessonById;

internal sealed class GetLessonByIdEndpoint : IEndpoint
{
    public const string Name = "GetLessonById";

    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/lessons/{lessonId:guid}", Handle)
            .WithName(Name)
            .WithTags("Lessons")
            .WithSummary("Get lesson by id")
            .RequireAuthorization();
    }

    private static async Task<Results<Ok<Lesson>, NotFound, ForbidHttpResult>> Handle(
        [FromRoute] Guid lessonId,
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetLessonByIdRequest(lessonId, user.GetRequiredUserId(), user.IsInRole("admin"));
        var response = await mediator.Send(request, cancellationToken);

        return response.Status switch
        {
            LessonAccessStatus.Success => TypedResults.Ok(response.Lesson!),
            LessonAccessStatus.Forbidden => TypedResults.Forbid(),
            _ => TypedResults.NotFound(),
        };
    }
}
