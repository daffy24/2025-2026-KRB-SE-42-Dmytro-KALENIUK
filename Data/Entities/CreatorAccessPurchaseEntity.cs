using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace Data.Entities;

/// <summary>
/// Represents a creator access purchase.
/// </summary>
[Table("creator_access_purchases")]
public sealed class CreatorAccessPurchaseEntity
{
    /// <summary>
    /// The unique purchase identifier.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// The user identifier.
    /// </summary>
    [Column("user_id")]
    public Guid UserId { get; init; }

    /// <summary>
    /// The purchase creation timestamp.
    /// </summary>
    [Column("timespan")]
    public DateTimeOffset Timespan { get; init; }

    /// <summary>
    /// The amount charged for creator access.
    /// </summary>
    [Column("amount")]
    public decimal Amount { get; init; }

    /// <summary>
    /// The current purchase status.
    /// </summary>
    [Column("status")]
    public CreatorAccessStatus Status { get; set; }
}
