using System.Net;
using System.Net.Http.Json;
using Common.Models;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.CreatorAccess;

[NonParallelizable]
public sealed class CreatorAccessEndpointTests
{
    private static readonly Guid StudentId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    [Test]
    public async Task CreatorAccessFlow_ActivatesCreatorPermissionsAfterSimulatedPayment()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        SetDevelopmentUser(client, StudentId, "student");

        var forbiddenCourseResponse = await CreateCourse(client);
        var purchaseResponse = await client.PostAsync("/creator-access", content: null);
        var purchase = await purchaseResponse.Content.ReadFromJsonAsync<CreatorAccessPurchaseResponse>();

        SetDevelopmentUser(client, StudentId, "admin");
        var activateResponse = await client.PostAsync($"/creator-access/{purchase!.Id}/activate", content: null);
        var activatedPurchase = await activateResponse.Content.ReadFromJsonAsync<CreatorAccessPurchaseResponse>();

        SetDevelopmentUser(client, StudentId, "student");
        var createdCourseResponse = await CreateCourse(client);

        Assert.Multiple(() =>
        {
            Assert.That(forbiddenCourseResponse.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            Assert.That(purchaseResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(purchase.Status, Is.EqualTo(CreatorAccessStatus.Pending));
            Assert.That(activateResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(activatedPurchase?.Status, Is.EqualTo(CreatorAccessStatus.Active));
            Assert.That(createdCourseResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        });
    }

    [Test]
    public async Task ActivateCreatorAccess_ReturnsNotFound_WhenPurchaseDoesNotExist()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();
        var missingId = Guid.Parse("99999999-9999-9999-9999-999999999999");

        var response = await client.PostAsync($"/creator-access/{missingId}/activate", content: null);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private static Task<HttpResponseMessage> CreateCourse(HttpClient client)
    {
        return client.PostAsJsonAsync("/courses", new
        {
            name = "C# Basics",
            summary = "Introductory C# course.",
            description = "Detailed course description.",
            language = "en",
            price = 0,
            tags = new[] { "csharp", "basics" },
            status = PublicationStatus.Published,
        });
    }

    private static void SetDevelopmentUser(HttpClient client, Guid userId, string roles)
    {
        client.DefaultRequestHeaders.Remove("X-User-Id");
        client.DefaultRequestHeaders.Remove("X-Roles");
        client.DefaultRequestHeaders.Add("X-User-Id", userId.ToString());
        client.DefaultRequestHeaders.Add("X-Roles", roles);
    }

    private sealed class CreatorAccessPurchaseResponse
    {
        public Guid Id { get; init; }

        public CreatorAccessStatus Status { get; init; }
    }
}
