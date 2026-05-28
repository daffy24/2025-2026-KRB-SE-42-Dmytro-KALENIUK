namespace Application.Modules.Lessons.Models;

/// <summary>
/// Represents a response received after adding a new lesson.
/// </summary>
/// <param name="Data">The id of created lesson.</param>
public sealed record AddLessonResponse(Guid Data);
