using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Courses;

[NonParallelizable]
public sealed class UploadCoursePreviewImageEndpointTests : EndpointTestBase
{
    [Test]
    public async Task UploadCoursePreviewImage_ReturnsOk()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);

        using var imageContent = CreateFileContent(
            "file",
            "preview.png",
            "image/png",
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]);

        var response = await client.PostAsync($"/courses/{courseId}/preview-image", imageContent);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
