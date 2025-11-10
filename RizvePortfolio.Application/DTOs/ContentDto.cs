namespace RizvePortfolio.Application.DTOs;

public record SkillDto(Guid Id, string Name, string Category, int Proficiency);
public record CreateSkillDto(string Name, string Category, int Proficiency);
public record UpdateSkillDto(string Name, string Category, int Proficiency);

public record ServiceDto(Guid Id, string Title, string? ShortDesc);
public record CreateServiceDto(string Title, string? ShortDesc);
public record UpdateServiceDto(string Title, string? ShortDesc);

public record ExperienceDto(Guid Id, string Company, string Title, DateTime From, DateTime? To, string? Description);
public record CreateExperienceDto(string Company, string Title, DateTime From, DateTime? To, string? Description);
public record UpdateExperienceDto(string Company, string Title, DateTime From, DateTime? To, string? Description);

public record EducationDto(Guid Id, string Institution, string Degree, DateTime From, DateTime? To);
public record CreateEducationDto(string Institution, string Degree, DateTime From, DateTime? To);
public record UpdateEducationDto(string Institution, string Degree, DateTime From, DateTime? To);

public record ContactDto(Guid Id, string Name, string Email, string Message, DateTime CreatedAt);
public record CreateContactDto(string Name, string Email, string Message);
