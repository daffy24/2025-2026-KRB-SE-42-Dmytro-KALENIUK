using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Subscriptions;

[NonParallelizable]
public sealed class AddSubscriptionEndpointTests : EndpointTestBase
{
    [Test]
    public async Task AddSubscription_ReturnsCreated()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await factory.SeedCourse(creatorId: Guid.NewGuid(), price: 0);

        var response = await client.PostAsync($"/courses/{courseId}/subscriptions", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
    }

    [Test]
    public async Task AddSubscription_ReturnsConflict_WhenSubscriptionAlreadyExists()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await factory.SeedCourse(creatorId: Guid.NewGuid(), price: 0);

        await client.PostAsync($"/courses/{courseId}/subscriptions", content: null);
        var response = await client.PostAsync($"/courses/{courseId}/subscriptions", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task AddSubscription_ReturnsBadRequest_ForOwnCourse()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var ownCourseId = await CreateCourse(client);

        var response = await client.PostAsync($"/courses/{ownCourseId}/subscriptions", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task AddSubscription_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.PostAsync($"/courses/{missingId}/subscriptions", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}
