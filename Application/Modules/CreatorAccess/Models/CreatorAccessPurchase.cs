using Common.Models;

namespace Application.Modules.CreatorAccess.Models;

/// <summary>
/// Represents a creator access purchase.
/// </summary>
public sealed class CreatorAccessPurchase
{
    /// <summary>
    /// The unique purchase identifier.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The user identifier.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// The purchase creation timestamp.
    /// </summary>
    public DateTimeOffset Timespan { get; init; }

    /// <summary>
    /// The amount charged for creator access.
    /// </summary>
    public decimal Amount { get; init; }

    /// <summary>
    /// The current purchase status.
    /// </summary>
    public CreatorAccessStatus Status { get; init; }
}
