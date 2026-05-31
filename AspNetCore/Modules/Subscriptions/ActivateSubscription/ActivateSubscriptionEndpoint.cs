using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Subscriptions.Models;
using Application.Modules.Subscriptions.Requests;
using EducationPlatform.Authentication;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Subscriptions.ActivateSubscription;

internal sealed class ActivateSubscriptionEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/subscriptions/{subscriptionId:guid}/activate", Handle)
            .WithName("ActivateSubscription")
            .WithTags("Subscriptions")
            .WithSummary("Simulate successful subscription payment")
            .RequireAuthorization(AuthPolicies.AdminOnly);
    }

    private static async Task<Results<Ok<Subscription>, NotFound, Conflict<string>>> Handle(
        [FromRoute] Guid subscriptionId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ActivateSubscriptionCommand(subscriptionId), cancellationToken);

        return response.Status switch
        {
            ActivateSubscriptionStatus.Activated => TypedResults.Ok(response.Subscription!),
            ActivateSubscriptionStatus.AlreadyActive => TypedResults.Conflict("Subscription is already active."),
            ActivateSubscriptionStatus.SubscriptionNotFound => TypedResults.NotFound(),
            _ => TypedResults.Conflict("Subscription cannot be activated."),
        };
    }
}
