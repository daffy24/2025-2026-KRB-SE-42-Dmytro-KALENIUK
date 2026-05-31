using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Lessons;

[NonParallelizable]
public sealed class GetLessonsEndpointTests : EndpointTestBase
{
    [Test]
    public async Task GetLessons_ReturnsOk()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);
        await CreateLesson(client, courseId);

        var response = await client.GetAsync($"/courses/{courseId}/lessons");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
