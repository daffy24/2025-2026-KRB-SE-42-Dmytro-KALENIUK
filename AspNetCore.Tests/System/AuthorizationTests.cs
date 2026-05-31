using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.System;

[NonParallelizable]
public sealed class AuthorizationTests
{
    [Test]
    public async Task ProtectedEndpoint_ReturnsUnauthorized_WhenUserIsNotAuthenticated()
    {
        await using var factory = new EducationPlatformApiFactory(useDevelopmentAuth: false);
        var client = factory.CreateClient();

        var response = await client.GetAsync("/courses");

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}
