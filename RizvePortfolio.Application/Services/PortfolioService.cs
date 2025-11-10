using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Application.DTOs;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Application.Services;

public class PortfolioService(IRepository<PortfolioItem> repo, IUnitOfWork uow) : IPortfolioService
{
    private readonly IRepository<PortfolioItem> _repo = repo;
    private readonly IUnitOfWork _uow = uow;

    public async Task<PortfolioItemDto?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        return entity is null ? null : ToDto(entity);
    }

    public async Task<IReadOnlyList<PortfolioItemDto>> ListAsync(CancellationToken ct = default)
    {
        var items = await _repo.ListAsync(ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<PortfolioItemDto> CreateAsync(PortfolioItemDto dto, CancellationToken ct = default)
    {
        var entity = new PortfolioItem
        {
            Title = dto.Title,
            Summary = dto.Summary,
            Content = dto.Content,
            CategoryId = dto.CategoryId
        };
        await _repo.AddAsync(entity, ct);
        await _uow.SaveChangesAsync(ct);
        return ToDto(entity);
    }

    public async Task<PortfolioItemDto?> UpdateAsync(Guid id, PortfolioItemDto dto, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return null;
        entity.Title = dto.Title;
        entity.Summary = dto.Summary;
        entity.Content = dto.Content;
        entity.CategoryId = dto.CategoryId;
        _repo.Update(entity);
        await _uow.SaveChangesAsync(ct);
        return ToDto(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _repo.GetByIdAsync(id, ct);
        if (entity is null) return false;
        _repo.Delete(entity);
        await _uow.SaveChangesAsync(ct);
        return true;
    }

    private static PortfolioItemDto ToDto(PortfolioItem e)
        => new(e.Id, e.Title, e.Summary, e.Content, e.CategoryId);
}
