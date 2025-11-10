using System.Threading;
using Microsoft.AspNetCore.Http;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Application.Services;

public interface ICvService
{
    Task<CvVersion> UploadCvAsync(IFormFile file, string uploadedBy, CancellationToken ct = default);
    Task<CvVersion?> GetActiveCvAsync(CancellationToken ct = default);
    Task RecordDownloadAsync(string ipHash, string userAgent, CancellationToken ct = default);
    Task<int> GetTotalDownloadCountAsync(CancellationToken ct = default);
}
