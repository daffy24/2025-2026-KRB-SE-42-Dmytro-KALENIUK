using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Courses;

[NonParallelizable]
public sealed class GetCoursePreviewImageEndpointTests : EndpointTestBase
{
    [Test]
    public async Task GetCoursePreviewImage_ReturnsImage()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);

        using var imageContent = CreateFileContent(
            "file",
            "preview.png",
            "image/png",
            [0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A]);
        await client.PostAsync($"/courses/{courseId}/preview-image", imageContent);

        var response = await client.GetAsync($"/courses/{courseId}/preview-image");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content.Headers.ContentType?.MediaType, Is.EqualTo("image/png"));
        });
    }

    [Test]
    public async Task GetCoursePreviewImage_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.GetAsync($"/courses/{missingId}/preview-image");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
