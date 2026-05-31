using Application.Modules.CreatorAccess.Models;
using MediatR;

namespace Application.Modules.CreatorAccess.Requests;

public sealed record AddCreatorAccessPurchaseCommand(Guid UserId) : IRequest<AddCreatorAccessPurchaseResult>;

public sealed record AddCreatorAccessPurchaseResult(
    AddCreatorAccessPurchaseStatus Status,
    CreatorAccessPurchase? Purchase = null);

public enum AddCreatorAccessPurchaseStatus
{
    Created,
    AlreadyCreator,
    AlreadyPurchased,
}
