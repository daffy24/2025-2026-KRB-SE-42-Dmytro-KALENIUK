using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.CreatorAccess.Models;
using Application.Modules.CreatorAccess.Requests;
using EducationPlatform.Authentication;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.CreatorAccess.ActivateCreatorAccessPurchase;

internal sealed class ActivateCreatorAccessPurchaseEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/creator-access/{purchaseId:guid}/activate", Handle)
            .WithName("ActivateCreatorAccessPurchase")
            .WithTags("Creator Access")
            .WithSummary("Simulate successful creator access payment")
            .RequireAuthorization(AuthPolicies.AdminOnly);
    }

    private static async Task<Results<Ok<CreatorAccessPurchase>, NotFound, Conflict<string>>> Handle(
        [FromRoute] Guid purchaseId,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new ActivateCreatorAccessPurchaseCommand(purchaseId), cancellationToken);

        return response.Status switch
        {
            ActivateCreatorAccessPurchaseStatus.Activated => TypedResults.Ok(response.Purchase!),
            ActivateCreatorAccessPurchaseStatus.AlreadyActive => TypedResults.Conflict("Creator access is already active."),
            ActivateCreatorAccessPurchaseStatus.PurchaseNotFound => TypedResults.NotFound(),
            _ => TypedResults.Conflict("Creator access cannot be activated."),
        };
    }
}
