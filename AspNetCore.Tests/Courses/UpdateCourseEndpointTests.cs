using System.Net;
using System.Net.Http.Json;
using Common.Models;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Courses;

[NonParallelizable]
public sealed class UpdateCourseEndpointTests : EndpointTestBase
{
    [Test]
    public async Task UpdateCourse_ReturnsOk()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await CreateCourse(client);

        var response = await client.PutAsJsonAsync($"/courses/{courseId}", new
        {
            name = "Updated C# Basics",
            summary = "Updated course summary.",
            description = "Updated course description.",
            language = "en",
            price = 0,
            tags = new[] { "csharp", "updated" },
            status = PublicationStatus.Published,
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
