using System;
using Application.Modules.Lessons.Requests;
using Common.Models;

namespace EducationPlatform.Modules.Lessons.AddLesson;

public sealed record AddLessonRequest(
    string Name,
    string Summary,
    string Description,
    PublicationStatus Status,
    TimeSpan Duration,
    MediaType MediaType)
{
    public AddLessonCommand ToRequest(Guid courseId, Guid creatorId)
    {
        return new AddLessonCommand(
            courseId,
            creatorId,
            Name,
            Summary,
            Description,
            Status,
            Duration,
            MediaType);
    }
}
