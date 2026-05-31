using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

[NonParallelizable]
public sealed class AddLessonEndpointTests : EndpointTestBase
{
    [Test]
    public async Task AddLesson_ReturnsCreated()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);

        var lessonId = await CreateLesson(client, courseId);

        Assert.That(lessonId, Is.Not.EqualTo(Guid.Empty));
    }
}
