using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Courses;

[NonParallelizable]
public sealed class GetCoursesEndpointTests : EndpointTestBase
{
    [Test]
    public async Task GetCourses_ReturnsOk()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        await CreateCourse(client);

        var response = await client.GetAsync("/courses");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
