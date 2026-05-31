using Application.Modules.Lessons.Handlers;
using Application.Modules.Lessons.Requests;
using Common.Models;
using EducationPlatformTests.TestSupport;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatformTests.Lessons;

public sealed class AddLessonHandlerTests
{
    [Test]
    public async Task AddLesson_PersistsCourseTextAndMediaType()
    {
        await using var dbContext = LessonTestSupport.CreateDbContext();
        var course = LessonTestSupport.CreateCourse(Guid.NewGuid());
        dbContext.Courses.Add(course);
        await dbContext.SaveChangesAsync();

        var handler = new AddLessonHandler(dbContext);
        var command = new AddLessonCommand(
            course.Id,
            course.CreatorId,
            "Variables and Types",
            "Basic variables and primitive types.",
            "Lesson description.",
            "Full lesson text.",
            PublicationStatus.Published,
            TimeSpan.FromMinutes(15),
            MediaType.Video);

        var response = await handler.Handle(command, CancellationToken.None);

        var lesson = await dbContext.Lessons.SingleAsync(x => x.Id == response.Data);
        Assert.Multiple(() =>
        {
            Assert.That(lesson.CourseId, Is.EqualTo(course.Id));
            Assert.That(lesson.Text, Is.EqualTo("Full lesson text."));
            Assert.That(lesson.MediaType, Is.EqualTo(MediaType.Video));
        });
    }
}
