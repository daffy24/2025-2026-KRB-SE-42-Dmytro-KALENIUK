using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EducationPlatform.Files;

internal interface IFileStorage
{
    Task<StoredFile> SaveAsync(IFormFile file, string folder, CancellationToken cancellationToken);

    string GetAbsolutePath(string relativePath);

    void Delete(string? relativePath);
}
