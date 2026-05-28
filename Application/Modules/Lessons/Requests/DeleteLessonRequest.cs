using MediatR;

namespace Application.Modules.Lessons.Requests;

public sealed record DeleteLessonRequest(Guid LessonId, Guid CreatorId) : IRequest<bool>;
