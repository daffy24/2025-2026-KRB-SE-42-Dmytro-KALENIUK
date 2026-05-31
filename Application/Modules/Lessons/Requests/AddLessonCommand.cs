using Application.Modules.Lessons.Models;
using Common.Models;
using MediatR;

namespace Application.Modules.Lessons.Requests;

public sealed record AddLessonCommand(
    Guid CourseId,
    Guid CreatorId,
    string Name,
    string Summary,
    string Description,
    string Text,
    PublicationStatus Status,
    TimeSpan Duration,
    MediaType MediaType) : IRequest<AddLessonResponse>;
