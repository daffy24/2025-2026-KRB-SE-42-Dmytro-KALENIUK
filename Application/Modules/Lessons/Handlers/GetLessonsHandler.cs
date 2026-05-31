using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using Common.Models;
using Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Modules.Lessons.Handlers;

internal sealed class GetLessonsHandler(EducationDbContext dbContext)
    : IRequestHandler<GetLessonsRequest, GetLessonsResult>
{
    public async Task<GetLessonsResult> Handle(GetLessonsRequest request, CancellationToken cancellationToken)
    {
        var course = await dbContext.Courses
            .AsNoTracking()
            .Where(x => x.Id == request.CourseId)
            .Select(x => new
            {
                x.Id,
                x.CreatorId,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (course is null)
            return new GetLessonsResult(LessonAccessStatus.NotFound, []);

        if (!request.CanManageAllLessons &&
            course.CreatorId != request.UserId &&
            !await HasActiveSubscription(course.Id, request.UserId, cancellationToken))
        {
            return new GetLessonsResult(LessonAccessStatus.Forbidden, []);
        }

        var lessons = await dbContext.Lessons
            .AsNoTracking()
            .Where(x => x.CourseId == request.CourseId)
            .Select(x => new Lesson
            {
                Id = x.Id,
                CourseId = x.CourseId,
                Name = x.Name,
                Summary = x.Summary,
                Description = x.Description,
                Text = x.Text,
                Status = x.Status,
                Duration = x.Duration,
                MediaType = x.MediaType,
                VideoUrl = x.VideoPath == null ? null : $"/lessons/{x.Id}/video",
                CreateTimestamp = x.CreateTimestamp,
            })
            .ToListAsync(cancellationToken);

        return new GetLessonsResult(LessonAccessStatus.Success, lessons);
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
}
