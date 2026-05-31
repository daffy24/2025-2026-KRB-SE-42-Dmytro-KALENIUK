using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Modules.CreatorAccess.Models;
using Application.Modules.CreatorAccess.Requests;
using EducationPlatform.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace EducationPlatform.Modules.CreatorAccess.AddCreatorAccessPurchase;

internal sealed class AddCreatorAccessPurchaseEndpoint : IEndpoint
{
    public RouteHandlerBuilder Map(IEndpointRouteBuilder app)
    {
        return app.MapPost("/creator-access", Handle)
            .WithName("AddCreatorAccessPurchase")
            .WithTags("Creator Access")
            .WithSummary("Buy creator access")
            .RequireAuthorization();
    }

    private static async Task<Results<Created<CreatorAccessPurchase>, Conflict<string>>> Handle(
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(
            new AddCreatorAccessPurchaseCommand(user.GetRequiredUserId()),
            cancellationToken);

        return response.Status switch
        {
            AddCreatorAccessPurchaseStatus.Created => TypedResults.Created(
                $"/creator-access/{response.Purchase!.Id}",
                response.Purchase),
            AddCreatorAccessPurchaseStatus.AlreadyCreator => TypedResults.Conflict(
                "User already has creator access."),
            AddCreatorAccessPurchaseStatus.AlreadyPurchased => TypedResults.Conflict(
                "User already has a pending creator access purchase."),
            _ => TypedResults.Conflict("Creator access purchase cannot be created."),
        };
    }
}
