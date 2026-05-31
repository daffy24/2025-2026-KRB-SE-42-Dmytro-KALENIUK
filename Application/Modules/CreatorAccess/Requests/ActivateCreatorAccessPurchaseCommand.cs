using Application.Modules.CreatorAccess.Models;
using MediatR;

namespace Application.Modules.CreatorAccess.Requests;

public sealed record ActivateCreatorAccessPurchaseCommand(Guid PurchaseId) : IRequest<ActivateCreatorAccessPurchaseResult>;

public sealed record ActivateCreatorAccessPurchaseResult(
    ActivateCreatorAccessPurchaseStatus Status,
    CreatorAccessPurchase? Purchase = null);

public enum ActivateCreatorAccessPurchaseStatus
{
    Activated,
    PurchaseNotFound,
    AlreadyActive,
}
