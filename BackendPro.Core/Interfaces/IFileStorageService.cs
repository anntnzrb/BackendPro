namespace BackendPro.Core.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(string sourceFilePath, string folder, string fileName, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(string filePath, CancellationToken cancellationToken = default);
}
