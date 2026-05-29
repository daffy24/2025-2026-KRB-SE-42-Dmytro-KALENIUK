using MediatR;

namespace Application.Modules.Courses.Requests;

public sealed record DeleteCourseRequest(
    Guid CourseId,
    Guid UserId,
    bool CanManageAllCourses) : IRequest<bool>;
