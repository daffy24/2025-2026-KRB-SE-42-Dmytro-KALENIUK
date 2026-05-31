using Application.Modules.Lessons.Handlers;
using Application.Modules.Lessons.Models;
using Application.Modules.Lessons.Requests;
using Common.Models;
using Data.Entities;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

public sealed class GetLessonByIdHandlerTests
{
    [Test]
    public async Task GetLessonById_ReturnsForbidden_WhenUserHasNoActiveSubscription()
    {
        await using var dbContext = LessonTestSupport.CreateDbContext();
        var creatorId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var lesson = await LessonTestSupport.SeedLesson(dbContext, creatorId);

        var handler = new GetLessonByIdHandler(dbContext);
        var result = await handler.Handle(
            new GetLessonByIdRequest(lesson.Id, studentId, CanManageAllLessons: false),
            CancellationToken.None);

        Assert.That(result.Status, Is.EqualTo(LessonAccessStatus.Forbidden));
        Assert.That(result.Lesson, Is.Null);
    }

    [Test]
    public async Task GetLessonById_ReturnsLesson_WhenUserHasActiveSubscription()
    {
        await using var dbContext = LessonTestSupport.CreateDbContext();
        var creatorId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var lesson = await LessonTestSupport.SeedLesson(dbContext, creatorId, videoPath: "lesson-videos/demo.mp4");

        dbContext.Subscriptions.Add(new SubscriptionEntity
        {
            Id = Guid.NewGuid(),
            UserId = studentId,
            CourseId = lesson.CourseId!.Value,
            Timespan = DateTimeOffset.UtcNow,
            IsFree = true,
            Amount = 0,
            Status = SubscriptionStatus.Active,
        });
        await dbContext.SaveChangesAsync();

        var handler = new GetLessonByIdHandler(dbContext);
        var result = await handler.Handle(
            new GetLessonByIdRequest(lesson.Id, studentId, CanManageAllLessons: false),
            CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(LessonAccessStatus.Success));
            Assert.That(result.Lesson, Is.Not.Null);
            Assert.That(result.Lesson!.Text, Is.EqualTo("Full lesson text."));
            Assert.That(result.Lesson.VideoUrl, Is.EqualTo($"/lessons/{lesson.Id}/video"));
        });
    }
}
