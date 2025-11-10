using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Application.Abstractions;

public interface IVisitorRepository
{
    Task<Visitor?> GetByIdAsync(Guid visitorId, CancellationToken ct = default);
    Task<Visitor?> GetByIpHashAsync(string ipHash, CancellationToken ct = default);
    Task AddAsync(Visitor visitor, CancellationToken ct = default);
    Task<IReadOnlyList<Visitor>> GetRecentAsync(int count, CancellationToken ct = default);
}
