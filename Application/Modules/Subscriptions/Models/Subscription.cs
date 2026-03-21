using Application.Modules.Common;

namespace Application.Modules.Subscribrions.Models;

/// <summary>
/// Represents a course subscription.
/// </summary>
public sealed class Subscription
{
    /// <summary>
    /// The unique identifier of the subscription.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The user identifier of the wish item.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// The course identifier of the wish item.
    /// </summary>
    public Guid CourseId { get; init; }

    /// <summary>
    /// The subscription timespan.
    /// </summary>
    public DateTimeOffset Timespan { get; init; }

    /// <summary>
    /// Indicates whether the subscription is free.
    /// </summary>
    public bool IsFree { get; init; }

    /// <summary>
    /// The amount charged for the subscription.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// The current status of the subscription.
    /// </summary>
    public SubscriptionStatus Status { get; init; }
}