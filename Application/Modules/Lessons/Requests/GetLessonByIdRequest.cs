using Application.Modules.Lessons.Models;
using MediatR;

namespace Application.Modules.Lessons.Requests;

public sealed record GetLessonByIdRequest(
    Guid LessonId,
    Guid UserId,
    bool CanManageAllLessons) : IRequest<GetLessonByIdResult>;
