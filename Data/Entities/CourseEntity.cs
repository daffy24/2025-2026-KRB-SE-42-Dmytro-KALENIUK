using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Common.Models;

namespace Data.Entities;

/// <summary>
/// Represents course database entity.
/// </summary>
[Table("courses")]
public sealed class CourseEntity
{
    /// <summary>
    /// The unique course identifier.
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// The unique course creator identifier.
    /// </summary>
    [Column("creator_id")]
    public Guid CreatorId { get; set; }

    /// <summary>
    /// The name of the course.
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Short description of the course.
    /// </summary>
    [Column("summary")]
    public string Summary { get; set; } = null!;

    /// <summary>
    /// Full detailed description.
    /// </summary>
    [Column("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// The preview image file path.
    /// </summary>
    [Column("preview_image_path")]
    public string? PreviewImagePath { get; set; }

    /// <summary>
    /// The preview image content type.
    /// </summary>
    [Column("preview_image_content_type")]
    public string? PreviewImageContentType { get; set; }

    /// <summary>
    /// The language of the course
    /// </summary>
    [Column("language")]
    public string Language { get; set; } = null!;

    /// <summary>
    /// The current status of the course.
    /// </summary>
    [Column("status")]
    public PublicationStatus Status { get; set; }

    /// <summary>
    /// The price of the course.
    /// </summary>
    [Column("price")]
    public decimal Price { get; set; }

    /// <summary>
    /// The time when the course was created.
    /// </summary>
    [Column("created_at")]
    public DateTimeOffset CreateTimestamp { get; set; }

    /// <summary>
    /// The time when the course was updated.
    /// </summary>
    [Column("updated_at")]
    public DateTimeOffset UpdateTimestamp { get; set; }

    /// <summary>
    /// Collection of course tags.
    /// </summary>
    [Column("features")]
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// The list of the subscription.
    /// </summary>
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
    public ICollection<SubscriptionEntity>? Subscriptions { get; set; }

    /// <summary>
    /// The list of course lessons.
    /// </summary>
    [SuppressMessage("Usage", "CA2227:Collection properties should be read only")]
    public ICollection<LessonEntity>? Lessons { get; set; }
}
