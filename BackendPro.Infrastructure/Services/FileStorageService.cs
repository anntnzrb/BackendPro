using BackendPro.Core.Interfaces;

namespace BackendPro.Infrastructure.Services;

public class FileStorageService(string webRootPath) : IFileStorageService
{
    private readonly string _webRootPath = webRootPath;

    public async Task<string> SaveFileAsync(string sourceFilePath, string folder, string fileName, CancellationToken cancellationToken = default)
    {
        var uploadsFolder = Path.Combine(_webRootPath, "images", folder);
        Directory.CreateDirectory(uploadsFolder);

        var destinationPath = Path.Combine(uploadsFolder, fileName);

#pragma warning disable MA0004
        await using (var sourceStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true))
        await using (var destinationStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
#pragma warning restore MA0004
        {
            await sourceStream.CopyToAsync(destinationStream, cancellationToken).ConfigureAwait(false);
        }

        return $"/images/{folder}/{fileName}";
    }

    public Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Task.FromResult(false);
        }

        var fullPath = Path.Combine(_webRootPath, filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
