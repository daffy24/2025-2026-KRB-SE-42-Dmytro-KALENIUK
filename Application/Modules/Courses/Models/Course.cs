using Application.Modules.Lessons.Models;
using Common.Models;

namespace Application.Modules.Courses.Models;

/// <summary>
/// Represents a course.
/// </summary>
public sealed class Course
{
    /// <summary>
    /// The unique identifier of the course.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The unique identifier of the course creator.
    /// </summary>
    public Guid CreatorId { get; init; }

    /// <summary>
    /// The name of the course.
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The short description of the course.
    /// </summary>
    public string Summary { get; init; } = null!;

    /// <summary>
    /// Full detailed description.
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// The language of the course.
    /// </summary>
    public string Language { get; set; } = null!;

    /// <summary>
    /// The price of the course.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Collection of tags associated with the course.
    /// </summary>
    public IEnumerable<string> Tags { get; init; } = [];

    /// <summary>
    /// The current status of the course.
    /// </summary>
    public PublicationStatus Status { get; init; }
    
    /// <summary>
    /// Number of subscribers enrolled in the course.
    /// </summary>
    public int? Subscribers { get; set; }
    
    /// <summary>
    /// The timestamp when the course was last updated.
    /// </summary>
    public DateTimeOffset UpdateTimestamp { get; set; }

    /// <summary>
    /// Collection of lessons in the course.
    /// </summary>
    public IEnumerable<Lesson> Lessons { get; init; } = [];
}