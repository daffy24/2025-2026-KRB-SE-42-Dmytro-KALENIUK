using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
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
            .WithSummary("Get course lessons");
    }

    private static async Task<Ok<IReadOnlyCollection<Lesson>>> Handle(
        [FromRoute] Guid courseId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetLessonsRequest(courseId);
        var response = await mediator.Send(request, cancellationToken);

        return TypedResults.Ok(response);
    }
}
