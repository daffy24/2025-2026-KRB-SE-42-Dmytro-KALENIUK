using Application.Modules.Courses.Models;
using Common.Models;
using MediatR;

namespace Application.Modules.Courses.Requests;

public sealed record UpdateCourseCommand(
    Guid CourseId,
    Guid UserId,
    bool CanManageAllCourses,
    string Name,
    string Summary,
    string Description,
    string Language,
    decimal Price,
    IEnumerable<string> Tags,
    PublicationStatus Status) : IRequest<Course?>;
