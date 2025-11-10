using RizvePortfolio.Domain.Entities;

namespace RizvePortfolio.Application.Abstractions;

public interface IContentRepository
{
    // Skills
    Task<IReadOnlyList<Skill>> GetAllSkillsAsync(CancellationToken ct = default);
    Task<Skill?> GetSkillByIdAsync(Guid id, CancellationToken ct = default);
    Task AddSkillAsync(Skill skill, CancellationToken ct = default);
    Task UpdateSkillAsync(Skill skill, CancellationToken ct = default);
    Task DeleteSkillAsync(Guid id, CancellationToken ct = default);

    // Services
    Task<IReadOnlyList<Service>> GetAllServicesAsync(CancellationToken ct = default);
    Task<Service?> GetServiceByIdAsync(Guid id, CancellationToken ct = default);
    Task AddServiceAsync(Service service, CancellationToken ct = default);
    Task UpdateServiceAsync(Service service, CancellationToken ct = default);
    Task DeleteServiceAsync(Guid id, CancellationToken ct = default);

    // Experience
    Task<IReadOnlyList<Experience>> GetAllExperiencesAsync(CancellationToken ct = default);
    Task<Experience?> GetExperienceByIdAsync(Guid id, CancellationToken ct = default);
    Task AddExperienceAsync(Experience experience, CancellationToken ct = default);
    Task UpdateExperienceAsync(Experience experience, CancellationToken ct = default);
    Task DeleteExperienceAsync(Guid id, CancellationToken ct = default);

    // Education
    Task<IReadOnlyList<Education>> GetAllEducationsAsync(CancellationToken ct = default);
    Task<Education?> GetEducationByIdAsync(Guid id, CancellationToken ct = default);
    Task AddEducationAsync(Education education, CancellationToken ct = default);
    Task UpdateEducationAsync(Education education, CancellationToken ct = default);
    Task DeleteEducationAsync(Guid id, CancellationToken ct = default);

    // Contact
    Task<IReadOnlyList<Contact>> GetAllContactsAsync(CancellationToken ct = default);
    Task<Contact?> GetContactByIdAsync(Guid id, CancellationToken ct = default);
    Task AddContactAsync(Contact contact, CancellationToken ct = default);
}
