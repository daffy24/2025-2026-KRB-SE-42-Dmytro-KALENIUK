using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Courses.Models;
using Application.Modules.Courses.Requests;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Courses.GetCourses;

internal sealed class GetCoursesEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/courses", Handle)
            .WithName("GetCourses")
            .WithTags("Courses")
            .WithSummary("Get courses")
            .RequireAuthorization();
    }

    private static async Task<Ok<IReadOnlyCollection<Course>>> Handle(
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetCoursesRequest(), cancellationToken);

        return TypedResults.Ok(response);
    }
}
