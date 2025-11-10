using Microsoft.EntityFrameworkCore;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Infrastructure.Persistence;

public class ProjectRepository(AppDbContext db) : IProjectRepository
{
    private readonly AppDbContext _db = db;

    public async Task<Project?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Projects.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Project?> GetBySlugAsync(string slug, CancellationToken ct = default)
        => await _db.Projects.FirstOrDefaultAsync(p => p.Slug == slug, ct);

    public async Task<IReadOnlyList<Project>> GetAllAsync(bool publicOnly = true, CancellationToken ct = default)
    {
        var query = _db.Projects.AsQueryable();
        if (publicOnly)
            query = query.Where(p => p.IsPublic);
        return await query.OrderByDescending(p => p.CreatedAt).ToListAsync(ct);
    }

    public async Task AddAsync(Project project, CancellationToken ct = default)
    {
        await _db.Projects.AddAsync(project, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Project project, CancellationToken ct = default)
    {
        _db.Projects.Update(project);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var project = await GetByIdAsync(id, ct);
        if (project != null)
        {
            _db.Projects.Remove(project);
            await _db.SaveChangesAsync(ct);
        }
    }
}
