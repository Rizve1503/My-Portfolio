using Microsoft.EntityFrameworkCore;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Infrastructure.Persistence;

public class CvRepository(AppDbContext db) : ICvRepository
{
    private readonly AppDbContext _db = db;

    public async Task<CvVersion?> GetActiveVersionAsync(CancellationToken ct = default)
        => await _db.CvVersions.FirstOrDefaultAsync(cv => cv.IsActive, ct);

    public async Task<CvVersion?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.CvVersions.FirstOrDefaultAsync(cv => cv.Id == id, ct);

    public async Task<IReadOnlyList<CvVersion>> GetAllVersionsAsync(CancellationToken ct = default)
        => await _db.CvVersions.OrderByDescending(cv => cv.UploadedAt).ToListAsync(ct);

    public async Task AddVersionAsync(CvVersion version, CancellationToken ct = default)
    {
        await _db.CvVersions.AddAsync(version, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<CvDownload> RecordDownloadAsync(CvDownload download, CancellationToken ct = default)
    {
        await _db.CvDownloads.AddAsync(download, ct);
        await _db.SaveChangesAsync(ct);
        return download;
    }

    public async Task<IReadOnlyList<CvDownload>> GetDownloadHistoryAsync(CancellationToken ct = default)
        => await _db.CvDownloads
            .Include(cd => cd.CvVersion)
            .OrderByDescending(cd => cd.DownloadedAt)
            .ToListAsync(ct);
}
