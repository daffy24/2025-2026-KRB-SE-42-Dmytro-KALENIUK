using Application.Modules.Subscriptions.Models;
using MediatR;

namespace Application.Modules.Subscriptions.Requests;

public sealed record ActivateSubscriptionCommand(Guid SubscriptionId) : IRequest<ActivateSubscriptionResult>;

public sealed record ActivateSubscriptionResult(
    ActivateSubscriptionStatus Status,
    Subscription? Subscription = null);

public enum ActivateSubscriptionStatus
{
    Activated,
    SubscriptionNotFound,
    AlreadyActive,
}
