using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EducationPlatform.Files;

internal sealed class LocalFileStorage(IWebHostEnvironment environment) : IFileStorage
{
    private readonly string storageRoot = Path.Combine(environment.ContentRootPath, "storage");

    public async Task<StoredFile> SaveAsync(
        IFormFile file,
        string folder,
        CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid():N}{extension}";
        var relativePath = $"{folder}/{fileName}";
        var absolutePath = GetAbsolutePath(relativePath);

        Directory.CreateDirectory(Path.GetDirectoryName(absolutePath)!);

        await using var stream = File.Create(absolutePath);
        await file.CopyToAsync(stream, cancellationToken);

        var contentType = string.IsNullOrWhiteSpace(file.ContentType)
            ? "application/octet-stream"
            : file.ContentType;

        return new StoredFile(relativePath, contentType);
    }

    public string GetAbsolutePath(string relativePath)
    {
        var absolutePath = Path.GetFullPath(Path.Combine(storageRoot, relativePath));
        var root = Path.GetFullPath(storageRoot);

        if (!absolutePath.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("File path is outside of the storage root.");

        return absolutePath;
    }

    public void Delete(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return;

        var absolutePath = GetAbsolutePath(relativePath);

        if (File.Exists(absolutePath))
            File.Delete(absolutePath);
    }
}
