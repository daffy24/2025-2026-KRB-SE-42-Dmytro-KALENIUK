using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

[NonParallelizable]
public sealed class GetLessonByIdEndpointTests : EndpointTestBase
{
    [Test]
    public async Task GetLessonById_ReturnsLesson()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);
        var lessonId = await CreateLesson(client, courseId);

        var response = await client.GetAsync($"/lessons/{lessonId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task GetLessonById_ReturnsNotFound_WhenLessonDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.GetAsync($"/lessons/{missingId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
