using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Application.Abstractions;

public interface ICvRepository
{
    Task<CvVersion?> GetActiveVersionAsync(CancellationToken ct = default);
    Task<CvVersion?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<CvVersion>> GetAllVersionsAsync(CancellationToken ct = default);
    Task AddVersionAsync(CvVersion version, CancellationToken ct = default);
    Task<CvDownload> RecordDownloadAsync(CvDownload download, CancellationToken ct = default);
    Task<IReadOnlyList<CvDownload>> GetDownloadHistoryAsync(CancellationToken ct = default);
}
