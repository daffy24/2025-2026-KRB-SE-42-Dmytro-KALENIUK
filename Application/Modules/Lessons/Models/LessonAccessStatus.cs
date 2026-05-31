namespace Application.Modules.Lessons.Models;

/// <summary>
/// Represents lesson access evaluation status.
/// </summary>
public enum LessonAccessStatus
{
    /// <summary>
    /// Lesson content is available.
    /// </summary>
    Success,

    /// <summary>
    /// The requested course or lesson was not found.
    /// </summary>
    NotFound,

    /// <summary>
    /// The user is not subscribed to the lesson course.
    /// </summary>
    Forbidden,
}
