using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

[NonParallelizable]
public sealed class GetLessonVideoEndpointTests : EndpointTestBase
{
    [Test]
    public async Task GetLessonVideo_ReturnsVideo()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);
        var lessonId = await CreateLesson(client, courseId);

        using var videoContent = CreateFileContent(
            "file",
            "lesson.mp4",
            "video/mp4",
            [0x00, 0x00, 0x00, 0x18, 0x66, 0x74, 0x79, 0x70]);
        await client.PostAsync($"/lessons/{lessonId}/video", videoContent);

        var response = await client.GetAsync($"/lessons/{lessonId}/video");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("video/mp4"));
        });
    }

    [Test]
    public async Task GetLessonVideo_ReturnsNotFound_WhenLessonDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.GetAsync($"/lessons/{missingId}/video");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
