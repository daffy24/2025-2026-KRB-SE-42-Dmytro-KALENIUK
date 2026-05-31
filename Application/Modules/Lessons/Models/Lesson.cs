
using Common.Models;

namespace Application.Modules.Lessons.Models;

/// <summary>
/// Represents a course lesson.
/// </summary>
public sealed class Lesson
{
    /// <summary>
    /// The unique identifier of the lesson.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The unique identifier of the course.
    /// </summary>
    public Guid? CourseId { get; init; }

    /// <summary>
    /// The name of the lesson.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// A short summary of the lesson.
    /// </summary>
    public string Summary { get; init; } = null!;

    /// <summary>
    /// The description of the lesson.
    /// </summary>
    public string Description { get; init; } = null!;

    /// <summary>
    /// The lesson text content.
    /// </summary>
    public string Text { get; init; } = string.Empty;

    /// <summary>
    /// The current status of the lesson.
    /// </summary>
    public PublicationStatus Status { get; init; }

    /// <summary>
    /// The duration of the lesson content.
    /// </summary>
    public TimeSpan Duration { get; init; }

    /// <summary>
    /// The type of media used for this lesson.
    /// </summary>
    public MediaType MediaType { get; init; }

    /// <summary>
    /// The URL of the protected lesson video endpoint.
    /// </summary>
    public string? VideoUrl { get; init; }

    /// <summary>
    /// The timestamp when the lesson was created.
    /// </summary>
    public DateTimeOffset CreateTimestamp { get; init; }
}
