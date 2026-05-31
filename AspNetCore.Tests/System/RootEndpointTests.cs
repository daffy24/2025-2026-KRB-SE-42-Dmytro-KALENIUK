using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.System;

[NonParallelizable]
public sealed class RootEndpointTests
{
    [Test]
    public async Task Root_ReturnsHelloWorld()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.EqualTo("Hello World!"));
        });
    }
}
