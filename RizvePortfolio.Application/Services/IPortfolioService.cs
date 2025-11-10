using RizvePortfolio.Application.DTOs;

namespace RizvePortfolio.Application.Services;

public interface IPortfolioService
{
    Task<PortfolioItemDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<PortfolioItemDto>> ListAsync(CancellationToken ct = default);
    Task<PortfolioItemDto> CreateAsync(PortfolioItemDto dto, CancellationToken ct = default);
    Task<PortfolioItemDto?> UpdateAsync(Guid id, PortfolioItemDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
