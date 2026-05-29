using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.Subscriptions.Models;
using Application.Modules.Subscriptions.Requests;
using EducationPlatform.Extensions;
using EducationPlatform.Modules;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.Subscriptions.GetSubscriptions;

internal sealed class GetSubscriptionsEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapGet("/subscriptions", Handle)
            .WithName("GetSubscriptions")
            .WithTags("Subscriptions")
            .WithSummary("Get current user subscriptions")
            .RequireAuthorization();
    }

    private static async Task<Ok<IReadOnlyCollection<Subscription>>> Handle(
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetSubscriptionsRequest(user.GetRequiredUserId()), cancellationToken);

        return TypedResults.Ok(response);
    }
}
