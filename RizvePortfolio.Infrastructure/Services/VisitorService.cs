using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RizvePortfolio.Application.Services;
using RizvePortfolio.Domain.Entities;
using RizvePortfolio.Infrastructure.Persistence;

namespace RizvePortfolio.Infrastructure.Services;

public class VisitorService : IVisitorService
{
    private static readonly TimeSpan VisitWindow = TimeSpan.FromMinutes(30);

    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<VisitorService> _logger;

    public VisitorService(
        AppDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ILogger<VisitorService> logger)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task TrackVisitAsync(string path, string referrer, string userAgent, string ipAddressHash)
    {
        if (IsAdminUser())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(ipAddressHash))
        {
            return;
        }

        var threshold = DateTime.UtcNow - VisitWindow;
        var alreadyVisited = await _dbContext.Visitors
            .AnyAsync(v => v.IpHash == ipAddressHash && v.CreatedAt >= threshold);

        if (alreadyVisited)
        {
            return;
        }

        var visitor = new Visitor
        {
            IpHash = ipAddressHash,
            Path = path,
            Referrer = string.IsNullOrWhiteSpace(referrer) ? null : referrer,
            UserAgent = string.IsNullOrWhiteSpace(userAgent) ? null : userAgent,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await _dbContext.Visitors.AddAsync(visitor);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to track visitor for path {Path}", path);
        }
    }

    public async Task<bool> IsRecentVisitAsync(string ipAddressHash, TimeSpan window)
    {
        if (string.IsNullOrWhiteSpace(ipAddressHash))
        {
            return false;
        }

        if (window <= TimeSpan.Zero)
        {
            window = VisitWindow;
        }

        var threshold = DateTime.UtcNow - window;
        return await _dbContext.Visitors
            .AnyAsync(v => v.IpHash == ipAddressHash && v.CreatedAt >= threshold);
    }

    private bool IsAdminUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        return user?.Identity is not null && user.Identity.IsAuthenticated && user.IsInRole("Admin");
    }
}
