using System.Net;
using EducationPlatformTests.TestSupport;

namespace EducationPlatformTests.System;

[NonParallelizable]
public sealed class SwaggerEndpointTests
{
    [Test]
    public async Task Swagger_ReturnsOpenApiDocumentAndUi()
    {
        await using var factory = new EducationPlatformApiFactory();
        var client = factory.CreateClient();

        var document = await client.GetAsync("/swagger/v1/swagger.json");
        var ui = await client.GetAsync("/swagger");

        Assert.Multiple(() =>
        {
            Assert.That(document.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(ui.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        });
    }
}
