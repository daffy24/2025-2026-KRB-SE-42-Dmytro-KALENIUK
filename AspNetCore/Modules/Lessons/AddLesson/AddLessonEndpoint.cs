using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Lessons.Models;
using Data;
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
using Microsoft.EntityFrameworkCore;

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

    private static async Task<Results<CreatedAtRoute<AddLessonResponse>, NotFound, ForbidHttpResult>> Handle(
        [FromRoute] Guid courseId,
        [FromBody] AddLessonRequest model,
        ClaimsPrincipal creator,
        IValidator<AddLessonRequest> validator,
        EducationDbContext dbContext,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(model, cancellationToken);

        var creatorId = creator.GetRequiredUserId();
        var course = await dbContext.Courses
            .AsNoTracking()
            .Where(x => x.Id == courseId)
            .Select(x => new { x.CreatorId })
            .FirstOrDefaultAsync(cancellationToken);

        if (course is null)
            return TypedResults.NotFound();

        if (!creator.IsInRole("admin") && course.CreatorId != creatorId)
            return TypedResults.Forbid();

        var request = model.ToRequest(courseId, creatorId);
        var response = await mediator.Send(request, cancellationToken);

        return TypedResults.CreatedAtRoute(response, GetLessonByIdEndpoint.Name, new { lessonId = response.Data });
    }
}
