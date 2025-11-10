using Microsoft.Extensions.Configuration;
using RizvePortfolio.Application.Abstractions;

namespace RizvePortfolio.Infrastructure.Services;

public class FileStorageService : IFileStorage
{
    private readonly string _uploadsPath;

    public FileStorageService(IConfiguration configuration)
    {
        _uploadsPath = configuration["UploadsPath"] ?? "uploads";
        
        // Ensure uploads directory exists
        if (!Directory.Exists(_uploadsPath))
        {
            Directory.CreateDirectory(_uploadsPath);
        }
    }

    public Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken ct = default)
        => SaveFileInternalAsync(fileStream, fileName, null, ct);

    public Task<string> SaveFileAsync(Stream fileStream, string fileName, string subDirectory, CancellationToken ct = default)
        => SaveFileInternalAsync(fileStream, fileName, subDirectory, ct);

    public Task<bool> DeleteFileAsync(string filePath, CancellationToken ct = default)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> FileExistsAsync(string filePath, CancellationToken ct = default)
    {
        return Task.FromResult(File.Exists(filePath));
    }

    public string GetFileUrl(string filePath)
    {
        // Return relative path for serving via static files
        return filePath.Replace("\\", "/");
    }
    private async Task<string> SaveFileInternalAsync(Stream fileStream, string fileName, string? subDirectory, CancellationToken ct)
    {
        var safeFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeFileName))
        {
            safeFileName = "file";
        }

        var targetDirectory = _uploadsPath;
        if (!string.IsNullOrWhiteSpace(subDirectory))
        {
            var cleanedSubDirectory = SanitizeSubDirectory(subDirectory);
            targetDirectory = Path.Combine(_uploadsPath, cleanedSubDirectory);
        }

        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{safeFileName}";
        var filePath = Path.Combine(targetDirectory, uniqueFileName);

        await using var fileStreamOut = File.Create(filePath);
        await fileStream.CopyToAsync(fileStreamOut, ct);

        return filePath;
    }

    private static string SanitizeSubDirectory(string subDirectory)
    {
        var trimmed = subDirectory.Replace("\\", "/", StringComparison.Ordinal);
        trimmed = string.Join('/', trimmed.Split('/', StringSplitOptions.RemoveEmptyEntries));
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return string.Empty;
        }

        if (trimmed.Contains("..", StringComparison.Ordinal))
        {
            trimmed = trimmed.Replace("..", string.Empty, StringComparison.Ordinal);
        }

        return trimmed;
    }
}
