using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Courses.Models;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using EducationPlatform.Modules.Courses.GetCourseById;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Courses.AddCourse;

internal sealed class AddCourseEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/courses", Handle)
            .WithName("AddCourse")
            .WithTags("Courses")
            .WithSummary("Add a new course")
            .RequireAuthorization();
    }

    private static async Task<CreatedAtRoute<AddCourseResponse>> Handle(
        [FromBody] AddCourseRequest model,
        ClaimsPrincipal creator,
        IValidator<AddCourseRequest> validator,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        var request = model.ToRequest(creator.GetRequiredUserId());
        var response = await mediator.Send(request, cancellationToken);

        return TypedResults.CreatedAtRoute(response, GetCourseByIdEndpoint.Name, new { courseId = response.Data });
    }
}
