using Microsoft.EntityFrameworkCore;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Infrastructure.Persistence;

public class VisitorRepository(AppDbContext db) : IVisitorRepository
{
    private readonly AppDbContext _db = db;

    public async Task<Visitor?> GetByIdAsync(Guid visitorId, CancellationToken ct = default)
        => await _db.Visitors.FirstOrDefaultAsync(v => v.VisitorId == visitorId, ct);

    public async Task<Visitor?> GetByIpHashAsync(string ipHash, CancellationToken ct = default)
        => await _db.Visitors.FirstOrDefaultAsync(v => v.IpHash == ipHash, ct);

    public async Task AddAsync(Visitor visitor, CancellationToken ct = default)
    {
        await _db.Visitors.AddAsync(visitor, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Visitor>> GetRecentAsync(int count, CancellationToken ct = default)
        => await _db.Visitors.OrderByDescending(v => v.CreatedAt).Take(count).ToListAsync(ct);
}
