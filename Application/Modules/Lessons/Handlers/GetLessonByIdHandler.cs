using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using Common.Models;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Lessons.Handlers;

internal sealed class GetLessonByIdHandler(EducationDbContext dbContext)
    : IRequestHandler<GetLessonByIdRequest, GetLessonByIdResult>
{
    public async Task<GetLessonByIdResult> Handle(GetLessonByIdRequest request, CancellationToken cancellationToken)
    {
        var lesson = await dbContext.Lessons
            .AsNoTracking()
            .Where(x => x.Id == request.LessonId)
            .Select(x => new
            {
                Entity = x,
                CourseCreatorId = x.Course == null ? (Guid?)null : x.Course.CreatorId,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (lesson is null || lesson.Entity.CourseId is null || lesson.CourseCreatorId is null)
            return new GetLessonByIdResult(LessonAccessStatus.NotFound);

        if (!request.CanManageAllLessons &&
            lesson.CourseCreatorId != request.UserId &&
            !await HasActiveSubscription(lesson.Entity.CourseId.Value, request.UserId, cancellationToken))
        {
            return new GetLessonByIdResult(LessonAccessStatus.Forbidden);
        }

        return new GetLessonByIdResult(
            LessonAccessStatus.Success,
            Map(lesson.Entity));
    }

    private async Task<bool> HasActiveSubscription(
        Guid courseId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await dbContext.Subscriptions
            .AsNoTracking()
            .AnyAsync(
                x => x.CourseId == courseId &&
                     x.UserId == userId &&
                     x.Status == SubscriptionStatus.Active,
                cancellationToken);
    }

    private static Lesson Map(Data.Entities.LessonEntity entity)
    {
        return new Lesson
        {
            Id = entity.Id,
            CourseId = entity.CourseId,
            Name = entity.Name,
            Summary = entity.Summary,
            Description = entity.Description,
            Text = entity.Text,
            Status = entity.Status,
            Duration = entity.Duration,
            MediaType = entity.MediaType,
            VideoUrl = entity.VideoPath == null ? null : $"/lessons/{entity.Id}/video",
            CreateTimestamp = entity.CreateTimestamp,
        };
    }
}
