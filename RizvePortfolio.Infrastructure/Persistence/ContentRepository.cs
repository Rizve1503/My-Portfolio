using Microsoft.EntityFrameworkCore;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Infrastructure.Persistence;

public class ContentRepository(AppDbContext db) : IContentRepository
{
    private readonly AppDbContext _db = db;

    // Skills
    public async Task<IReadOnlyList<Skill>> GetAllSkillsAsync(CancellationToken ct = default)
        => await _db.Skills.ToListAsync(ct);

    public async Task<Skill?> GetSkillByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Skills.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task AddSkillAsync(Skill skill, CancellationToken ct = default)
    {
        await _db.Skills.AddAsync(skill, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateSkillAsync(Skill skill, CancellationToken ct = default)
    {
        _db.Skills.Update(skill);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteSkillAsync(Guid id, CancellationToken ct = default)
    {
        var skill = await GetSkillByIdAsync(id, ct);
        if (skill != null)
        {
            _db.Skills.Remove(skill);
            await _db.SaveChangesAsync(ct);
        }
    }

    // Services
    public async Task<IReadOnlyList<Service>> GetAllServicesAsync(CancellationToken ct = default)
        => await _db.Services.ToListAsync(ct);

    public async Task<Service?> GetServiceByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Services.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task AddServiceAsync(Service service, CancellationToken ct = default)
    {
        await _db.Services.AddAsync(service, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateServiceAsync(Service service, CancellationToken ct = default)
    {
        _db.Services.Update(service);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteServiceAsync(Guid id, CancellationToken ct = default)
    {
        var service = await GetServiceByIdAsync(id, ct);
        if (service != null)
        {
            _db.Services.Remove(service);
            await _db.SaveChangesAsync(ct);
        }
    }

    // Experience
    public async Task<IReadOnlyList<Experience>> GetAllExperiencesAsync(CancellationToken ct = default)
        => await _db.Experiences.OrderByDescending(e => e.From).ToListAsync(ct);

    public async Task<Experience?> GetExperienceByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Experiences.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task AddExperienceAsync(Experience experience, CancellationToken ct = default)
    {
        await _db.Experiences.AddAsync(experience, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateExperienceAsync(Experience experience, CancellationToken ct = default)
    {
        _db.Experiences.Update(experience);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteExperienceAsync(Guid id, CancellationToken ct = default)
    {
        var experience = await GetExperienceByIdAsync(id, ct);
        if (experience != null)
        {
            _db.Experiences.Remove(experience);
            await _db.SaveChangesAsync(ct);
        }
    }

    // Education
    public async Task<IReadOnlyList<Education>> GetAllEducationsAsync(CancellationToken ct = default)
        => await _db.Educations.OrderByDescending(e => e.From).ToListAsync(ct);

    public async Task<Education?> GetEducationByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Educations.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task AddEducationAsync(Education education, CancellationToken ct = default)
    {
        await _db.Educations.AddAsync(education, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateEducationAsync(Education education, CancellationToken ct = default)
    {
        _db.Educations.Update(education);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteEducationAsync(Guid id, CancellationToken ct = default)
    {
        var education = await GetEducationByIdAsync(id, ct);
        if (education != null)
        {
            _db.Educations.Remove(education);
            await _db.SaveChangesAsync(ct);
        }
    }

    // Contact
    public async Task<IReadOnlyList<Contact>> GetAllContactsAsync(CancellationToken ct = default)
        => await _db.Contacts.OrderByDescending(c => c.CreatedAt).ToListAsync(ct);

    public async Task<Contact?> GetContactByIdAsync(Guid id, CancellationToken ct = default)
        => await _db.Contacts.FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task AddContactAsync(Contact contact, CancellationToken ct = default)
    {
        await _db.Contacts.AddAsync(contact, ct);
        await _db.SaveChangesAsync(ct);
    }
}
