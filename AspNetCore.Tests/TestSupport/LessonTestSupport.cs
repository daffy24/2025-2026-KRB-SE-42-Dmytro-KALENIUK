using Common.Models;
using Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatformTests.TestSupport;

internal static class LessonTestSupport
{
    public static EducationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<EducationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new EducationDbContext(options);
    }

    public static async Task<LessonEntity> SeedLesson(
        EducationDbContext dbContext,
        Guid creatorId,
        string? videoPath = null)
    {
        var course = CreateCourse(creatorId);
        var lesson = new LessonEntity
        {
            Id = Guid.NewGuid(),
            CourseId = course.Id,
            CreatorId = creatorId,
            Name = "Variables and Types",
            Summary = "Basic variables and primitive types.",
            Description = "Lesson description.",
            Text = "Full lesson text.",
            Status = PublicationStatus.Published,
            Duration = TimeSpan.FromMinutes(15),
            MediaType = videoPath is null ? MediaType.None : MediaType.Video,
            VideoPath = videoPath,
            VideoContentType = videoPath is null ? null : "video/mp4",
            CreateTimestamp = DateTimeOffset.UtcNow,
            UpdateTimestamp = DateTimeOffset.UtcNow,
        };

        dbContext.Courses.Add(course);
        dbContext.Lessons.Add(lesson);
        await dbContext.SaveChangesAsync();

        return lesson;
    }

    public static CourseEntity CreateCourse(Guid creatorId)
    {
        var timestamp = DateTimeOffset.UtcNow;

        return new CourseEntity
        {
            Id = Guid.NewGuid(),
            CreatorId = creatorId,
            Name = "C# Basics",
            Summary = "Introductory C# course.",
            Description = "Detailed course description.",
            Language = "en",
            Status = PublicationStatus.Published,
            Price = 0,
            Tags = ["csharp", "basics"],
            CreateTimestamp = timestamp,
            UpdateTimestamp = timestamp,
        };
    }
}
