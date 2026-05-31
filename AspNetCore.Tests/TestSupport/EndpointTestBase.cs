using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Common.Models;

namespace EducationPlatformTests.TestSupport;

public abstract class EndpointTestBase
{
    protected static async Task<Guid> CreateCourse(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/courses", new
        {
            name = "C# Basics",
            summary = "Introductory C# course.",
            description = "Detailed course description.",
            language = "en",
            price = 0,
            tags = new[] { "csharp", "basics" },
            status = PublicationStatus.Published,
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        return await ReadResponseId(response);
    }

    protected static async Task<Guid> CreateLesson(HttpClient client, Guid courseId)
    {
        var response = await client.PostAsJsonAsync($"/courses/{courseId}/lessons", new
        {
            name = "Variables and Types",
            summary = "Basic variables and primitive types.",
            description = "Lesson description.",
            text = "Full lesson text.",
            status = PublicationStatus.Published,
            duration = "00:15:00",
            mediaType = MediaType.Video,
        });

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        return await ReadResponseId(response);
    }

    protected static async Task<Guid> ReadResponseId(HttpResponseMessage response)
    {
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        return document.RootElement.GetProperty("data").GetGuid();
    }

    protected static MultipartFormDataContent CreateFileContent(
        string fieldName,
        string fileName,
        string contentType,
        byte[] bytes)
    {
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(bytes);
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
        content.Add(fileContent, fieldName, fileName);
        return content;
    }
}
