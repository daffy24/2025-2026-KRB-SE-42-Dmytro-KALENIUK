using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Courses.Models;
using Application.Modules.Courses.Requests;
using EducationPlatform.Modules;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Courses.GetCourseById;

internal sealed class GetCourseByIdEndpoint : IEndpoint
{
    public const string Name = "GetCourseById";

    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/courses/{courseId:guid}", Handle)
            .WithName(Name)
            .WithTags("Courses")
            .WithSummary("Get course by id")
            .RequireAuthorization();
    }

    private static async Task<Results<Ok<Course>, NotFound>> Handle(
        [FromRoute] Guid courseId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new GetCourseByIdRequest(courseId);
        var response = await mediator.Send(request, cancellationToken);

        return response is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(response);
    }
}
