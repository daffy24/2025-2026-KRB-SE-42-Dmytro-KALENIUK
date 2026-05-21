using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;

namespace Data.Entities;

/// <summary>
/// Represents course lesson database entity.
/// </summary>
[Table("lessons")]
public sealed class LessonEntity
{
    /// <summary>
    /// The unique lesson identifier.
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
    /// The name of the lesson.
    /// </summary>
    [Column("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Short description of the lesson.
    /// </summary>
    [Column("summary")]
    public string Summary { get; set; } = null!;

    /// <summary>
    /// The description of the lesson.
    /// </summary>
    [Column("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// The current status of the lesson.
    /// </summary>
    [Column("status")]
    public PublicationStatus Status { get; set; }

    /// <summary>
    /// The time when the lesson was created.
    /// </summary>
    [Column("created_at")]
    public DateTimeOffset CreateTimestamp { get; set; }

    /// <summary>
    /// The time when the lesson was updated.
    /// </summary>
    [Column("updated_at")]
    public DateTimeOffset UpdateTimestamp { get; set; }

    /// <summary>
    /// The duration of the lesson.
    /// </summary>
    [Column("duration")]
    public TimeSpan Duration { get; set; }
}