using Application.Modules.Subscriptions.Models;
using MediatR;

namespace Application.Modules.Subscriptions.Requests;

public sealed record GetSubscriptionsRequest(Guid UserId) : IRequest<IReadOnlyCollection<Subscription>>;
