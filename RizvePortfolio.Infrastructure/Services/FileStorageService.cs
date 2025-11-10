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

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken ct = default)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(_uploadsPath, uniqueFileName);

        using var fileStreamOut = File.Create(filePath);
        await fileStream.CopyToAsync(fileStreamOut, ct);

        return filePath;
    }

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
}
