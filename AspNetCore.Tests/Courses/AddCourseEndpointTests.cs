using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Courses;

[NonParallelizable]
public sealed class AddCourseEndpointTests : EndpointTestBase
{
    [Test]
    public async Task AddCourse_ReturnsCreated()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();

        var courseId = await CreateCourse(client);

        Assert.That(courseId, Is.Not.EqualTo(Guid.Empty));
    }
}
