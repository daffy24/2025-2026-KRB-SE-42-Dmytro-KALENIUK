using System.Net;
using System.Net.Http.Json;
using Common.Models;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.Subscriptions;

[NonParallelizable]
public sealed class ActivateSubscriptionEndpointTests : EndpointTestBase
{
    [Test]
    public async Task ActivateSubscription_ReturnsOk_AndActivatesPendingSubscription()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await factory.SeedCourse(creatorId: Guid.NewGuid(), price: 100);

        var createResponse = await client.PostAsync($"/courses/{courseId}/subscriptions", content: null);
        var createdSubscription = await createResponse.Content.ReadFromJsonAsync<SubscriptionResponse>();
        var subscriptionId = createdSubscription!.Id;

        var activateResponse = await client.PostAsync($"/subscriptions/{subscriptionId}/activate", content: null);
        var subscription = await activateResponse.Content.ReadFromJsonAsync<SubscriptionResponse>();

        Assert.Multiple(() =>
        {
            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(activateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(subscription?.Status, Is.EqualTo(SubscriptionStatus.Active));
        });
    }

    [Test]
    public async Task ActivateSubscription_ReturnsConflict_WhenSubscriptionIsAlreadyActive()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var courseId = await factory.SeedCourse(creatorId: Guid.NewGuid(), price: 0);

        var createResponse = await client.PostAsync($"/courses/{courseId}/subscriptions", content: null);
        var createdSubscription = await createResponse.Content.ReadFromJsonAsync<SubscriptionResponse>();
        var subscriptionId = createdSubscription!.Id;

        var response = await client.PostAsync($"/subscriptions/{subscriptionId}/activate", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task ActivateSubscription_ReturnsNotFound_WhenSubscriptionDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.PostAsync($"/subscriptions/{missingId}/activate", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private sealed class SubscriptionResponse
    {
        public Guid Id { get; init; }

        public SubscriptionStatus Status { get; init; }
    }
}
