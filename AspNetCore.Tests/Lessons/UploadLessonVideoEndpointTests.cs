using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

[NonParallelizable]
public sealed class UploadLessonVideoEndpointTests : EndpointTestBase
{
    [Test]
    public async Task UploadLessonVideo_ReturnsOk()
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

        var response = await client.PostAsync($"/lessons/{lessonId}/video", videoContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
