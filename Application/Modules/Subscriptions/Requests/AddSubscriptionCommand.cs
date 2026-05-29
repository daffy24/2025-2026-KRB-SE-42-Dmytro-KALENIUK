using Application.Modules.Subscriptions.Models;
using MediatR;

namespace Application.Modules.Subscriptions.Requests;

public sealed record AddSubscriptionCommand(Guid UserId, Guid CourseId) : IRequest<AddSubscriptionResult>;

public sealed record AddSubscriptionResult(
    AddSubscriptionStatus Status,
    Subscription? Subscription = null);

public enum AddSubscriptionStatus
{
    Created,
    CourseNotFound,
    CourseNotPublished,
    OwnCourse,
    AlreadySubscribed,
}
