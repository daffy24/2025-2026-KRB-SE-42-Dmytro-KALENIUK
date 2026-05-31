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
    /// The unique course identifier.
    /// </summary>
    [Column("course_id")]
    public Guid? CourseId { get; set; }

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
    /// The lesson text content.
    /// </summary>
    [Column("text")]
    public string Text { get; set; } = string.Empty;

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

    /// <summary>
    /// The type of media used for this lesson.
    /// </summary>
    [Column("media_type")]
    public MediaType MediaType { get; set; }

    /// <summary>
    /// The lesson video file path.
    /// </summary>
    [Column("video_path")]
    public string? VideoPath { get; set; }

    /// <summary>
    /// The lesson video content type.
    /// </summary>
    [Column("video_content_type")]
    public string? VideoContentType { get; set; }

    /// <summary>
    /// The course that owns the lesson.
    /// </summary>
    public CourseEntity? Course { get; set; }
}
