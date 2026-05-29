using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Models;
using EducationPlatform.Authentication;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using EducationPlatform.Modules.Lessons.GetLessonById;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Lessons.AddLesson;

internal sealed class AddLessonEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/courses/{courseId:guid}/lessons", Handle)
            .WithName("AddLesson")
            .WithTags("Lessons")
            .WithSummary("Add a new lesson")
            .RequireAuthorization(AuthPolicies.CreatorOnly);
    }

    private static async Task<CreatedAtRoute<AddLessonResponse>> Handle(
        [FromRoute] Guid courseId,
        [FromBody] AddLessonRequest model,
        ClaimsPrincipal creator,
        IValidator<AddLessonRequest> validator,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        var request = model.ToRequest(courseId, creator.GetRequiredUserId());
        var response = await mediator.Send(request, cancellationToken);

        return TypedResults.CreatedAtRoute(response, GetLessonByIdEndpoint.Name, new { lessonId = response.Data });
    }
}
