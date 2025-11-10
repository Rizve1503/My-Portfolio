namespace RizvePortfolio.Application.Abstractions;

public interface IFileStorage
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken ct = default);
    Task<bool> DeleteFileAsync(string filePath, CancellationToken ct = default);
    Task<bool> FileExistsAsync(string filePath, CancellationToken ct = default);
    string GetFileUrl(string filePath);
}
