using System;
using System.Threading.Tasks;

namespace RizvePortfolio.Application.Services;

public interface IVisitorService
{
    Task TrackVisitAsync(string path, string referrer, string userAgent, string ipAddressHash);
    Task<bool> IsRecentVisitAsync(string ipAddressHash, TimeSpan window);
}
