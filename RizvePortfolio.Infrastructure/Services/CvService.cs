using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Application.Services;
using RizvePortfolio.Domain.Entities;
using RizvePortfolio.Infrastructure.Persistence;

namespace RizvePortfolio.Infrastructure.Services;

public class CvService : ICvService
{
    private static readonly TimeSpan DownloadWindow = TimeSpan.FromMinutes(30);
    private const string CvSubDirectory = "cv";

    private readonly AppDbContext _dbContext;
    private readonly IFileStorage _fileStorage;
    private readonly ILogger<CvService> _logger;

    public CvService(AppDbContext dbContext, IFileStorage fileStorage, ILogger<CvService> logger)
    {
        _dbContext = dbContext;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<CvVersion> UploadCvAsync(IFormFile file, string uploadedBy, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(file);

        if (file.Length <= 0)
        {
            throw new ArgumentException("File is empty.", nameof(file));
        }

        var uploader = string.IsNullOrWhiteSpace(uploadedBy) ? "Unknown" : uploadedBy.Trim();
        var safeFileName = Path.GetFileName(string.IsNullOrWhiteSpace(file.FileName) ? "cv.pdf" : file.FileName);

        await using var readStream = file.OpenReadStream();
        var storedPath = await _fileStorage.SaveFileAsync(readStream, safeFileName, CvSubDirectory, ct);

        await DeactivateExistingVersionsAsync(ct);

        var cvVersion = new CvVersion
        {
            FileName = safeFileName,
            FilePath = storedPath,
            UploadedAt = DateTime.UtcNow,
            UploadedBy = uploader,
            IsActive = true
        };

        await _dbContext.CvVersions.AddAsync(cvVersion, ct);
        await _dbContext.SaveChangesAsync(ct);

        return cvVersion;
    }

    public Task<CvVersion?> GetActiveCvAsync(CancellationToken ct = default)
        => _dbContext.CvVersions.AsNoTracking().FirstOrDefaultAsync(cv => cv.IsActive, ct);

    public async Task RecordDownloadAsync(string ipHash, string userAgent, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(ipHash))
        {
            return;
        }

        var activeCv = await _dbContext.CvVersions.FirstOrDefaultAsync(cv => cv.IsActive, ct);
        if (activeCv is null)
        {
            return;
        }

        var threshold = DateTime.UtcNow - DownloadWindow;
        var alreadyDownloaded = await _dbContext.CvDownloads.AnyAsync(
            cd => cd.CvVersionId == activeCv.Id && cd.IpHash == ipHash && cd.DownloadedAt >= threshold,
            ct);

        if (alreadyDownloaded)
        {
            return;
        }

        var download = new CvDownload
        {
            CvVersionId = activeCv.Id,
            IpHash = ipHash,
            UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent,
            DownloadedAt = DateTime.UtcNow
        };

        await _dbContext.CvDownloads.AddAsync(download, ct);

        try
        {
            await _dbContext.SaveChangesAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to record CV download for version {CvVersionId}", activeCv.Id);
        }
    }

    public Task<int> GetTotalDownloadCountAsync(CancellationToken ct = default)
        => _dbContext.CvDownloads.CountAsync(ct);

    private async Task DeactivateExistingVersionsAsync(CancellationToken ct)
    {
        try
        {
            await _dbContext.CvVersions
                .Where(cv => cv.IsActive)
                .ExecuteUpdateAsync(setters => setters.SetProperty(cv => cv.IsActive, _ => false), ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deactivate previous CV versions");
        }
    }
}
