using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace Data.Entities;

/// <summary>
/// Represents subscription database entity.
/// </summary>
[Table("subscriptions")]
public sealed class SubscriptionEntity
{
    /// <summary>
    /// The unique identifier of the subscription.
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
    /// The course identifier.
    /// </summary>
    [Column("course_id")]
    public Guid CourseId { get; init; }

    /// <summary>
    /// The subscription timespan.
    /// </summary>
    [Column("timespan")]
    public DateTimeOffset Timespan { get; init; }

    /// <summary>
    /// Indicates whether the subscription is free.
    /// </summary>
    [Column("is_free")]
    public bool IsFree { get; init; }

    /// <summary>
    /// The amount charged for the subscription.
    /// </summary>
    [Column("amount")]
    public decimal Amount { get; init; }

    /// <summary>
    /// The current status of the subscription.
    /// </summary>
    [Column("status")]
    public SubscriptionStatus Status { get; set; }

    /// <summary>
    /// The subscription course.
    /// </summary>
    public CourseEntity? Course { get; set; }
}
