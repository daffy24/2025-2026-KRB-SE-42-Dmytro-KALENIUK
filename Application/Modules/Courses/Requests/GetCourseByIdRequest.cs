using Application.Modules.Courses.Models;
using MediatR;

namespace Application.Modules.Courses.Requests;

public sealed record GetCourseByIdRequest(Guid CourseId) : IRequest<Course?>;
