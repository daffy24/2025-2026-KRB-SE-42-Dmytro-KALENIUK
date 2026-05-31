namespace Application.Modules.Lessons.Models;

/// <summary>
/// Represents the get lessons result.
/// </summary>
public sealed record GetLessonsResult(
    LessonAccessStatus Status,
    IReadOnlyCollection<Lesson> Lessons);
