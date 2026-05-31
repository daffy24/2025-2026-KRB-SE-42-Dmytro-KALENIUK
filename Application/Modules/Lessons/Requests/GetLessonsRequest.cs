using Application.Modules.Lessons.Models;
using MediatR;

namespace Application.Modules.Lessons.Requests;

public sealed record GetLessonsRequest(
    Guid CourseId,
    Guid UserId,
    bool CanManageAllLessons) : IRequest<GetLessonsResult>;
