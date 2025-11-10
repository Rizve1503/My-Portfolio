using Microsoft.EntityFrameworkCore;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Infrastructure.Persistence;

public class Repository<T>(AppDbContext db) : IRepository<T>, IUnitOfWork where T : BaseEntity
{
    private readonly AppDbContext _db = db;
    private readonly DbSet<T> _set = db.Set<T>();

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _set.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken ct = default)
        => await _set.AsNoTracking().ToListAsync(ct);

    public async Task AddAsync(T entity, CancellationToken ct = default)
        => await _set.AddAsync(entity, ct);

    public void Update(T entity) => _set.Update(entity);
    public void Delete(T entity) => _set.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
