using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Courses.Models;
using EducationPlatform.Authentication;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Courses.UpdateCourse;

internal sealed class UpdateCourseEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPut("/courses/{courseId:guid}", Handle)
            .WithName("UpdateCourse")
            .WithTags("Courses")
            .WithSummary("Update course")
            .RequireAuthorization(AuthPolicies.CreatorOnly);
    }

    private static async Task<Results<Ok<Course>, NotFound>> Handle(
        [FromRoute] Guid courseId,
        [FromBody] UpdateCourseRequest model,
        ClaimsPrincipal user,
        IValidator<UpdateCourseRequest> validator,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        var request = model.ToRequest(courseId, user.GetRequiredUserId(), user.IsInRole("admin"));
        var response = await mediator.Send(request, cancellationToken);

        return response is null
            ? TypedResults.NotFound()
            : TypedResults.Ok(response);
    }
}
