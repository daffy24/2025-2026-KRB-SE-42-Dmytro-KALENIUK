using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Subscriptions;

[NonParallelizable]
public sealed class GetSubscriptionsEndpointTests
{
    [Test]
    public async Task GetSubscriptions_ReturnsOk()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await factory.SeedCourse(creatorId: Guid.NewGuid(), price: 0);
        await client.PostAsync($"/courses/{courseId}/subscriptions", content: null);

        var response = await client.GetAsync("/subscriptions");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}
