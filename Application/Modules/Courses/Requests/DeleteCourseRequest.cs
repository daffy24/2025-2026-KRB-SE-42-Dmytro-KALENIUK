using MediatR;

namespace Application.Modules.Courses.Requests;

public sealed record DeleteCourseRequest(Guid CourseId, Guid CreatorId) : IRequest<bool>;
