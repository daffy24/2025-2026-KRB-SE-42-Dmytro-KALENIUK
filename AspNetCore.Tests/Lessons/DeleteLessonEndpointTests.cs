using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

[NonParallelizable]
public sealed class DeleteLessonEndpointTests : EndpointTestBase
{
    [Test]
    public async Task DeleteLesson_ReturnsNoContent()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);
        var lessonId = await CreateLesson(client, courseId);

        var response = await client.DeleteAsync($"/lessons/{lessonId}");
        var getDeletedResponse = await client.GetAsync($"/lessons/{lessonId}");

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            Assert.That(getDeletedResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        });
    }

    [Test]
    public async Task DeleteLesson_ReturnsNotFound_WhenLessonDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.DeleteAsync($"/lessons/{missingId}");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
