using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Subscriptions.Models;
using Application.Modules.Subscriptions.Requests;
using EducationPlatform.Authentication;
using EducationPlatform.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Subscriptions.AddSubscription;

internal sealed class AddSubscriptionEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/courses/{courseId:guid}/subscriptions", Handle)
            .WithName("AddSubscription")
            .WithTags("Subscriptions")
            .WithSummary("Subscribe to course")
            .RequireAuthorization(AuthPolicies.StudentOrCreatorOnly);
    }

    private static async Task<Results<Created<Subscription>, NotFound, BadRequest<string>, Conflict<string>>> Handle(
        [FromRoute] Guid courseId,
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var request = new AddSubscriptionCommand(user.GetRequiredUserId(), courseId);
        var response = await mediator.Send(request, cancellationToken);

        return response.Status switch
        {
            AddSubscriptionStatus.Created => TypedResults.Created(
                $"/subscriptions/{response.Subscription!.Id}",
                response.Subscription),
            AddSubscriptionStatus.CourseNotFound => TypedResults.NotFound(),
            AddSubscriptionStatus.CourseNotPublished => TypedResults.BadRequest(
                "Only published courses can be subscribed to."),
            AddSubscriptionStatus.OwnCourse => TypedResults.BadRequest(
                "Creators cannot subscribe to their own courses."),
            AddSubscriptionStatus.AlreadySubscribed => TypedResults.Conflict(
                "User is already subscribed to this course."),
            _ => TypedResults.BadRequest("Subscription cannot be created."),
        };
    }
}
