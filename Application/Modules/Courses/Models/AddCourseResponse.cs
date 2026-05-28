namespace Application.Modules.Courses.Models;

/// <summary>
/// Represents a response received after adding a new course.
/// </summary>
/// <param name="Data">The id of created course.</param>
public sealed record AddCourseResponse(Guid Data) : Response<Guid>(Data);
