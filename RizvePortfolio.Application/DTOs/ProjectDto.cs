namespace RizvePortfolio.Application.DTOs;

public record ProjectDto(
    Guid Id,
    string Title,
    string Slug,
    string? ShortDesc,
    string? LongDesc,
    string? TechTags,
    string? ThumbnailPath,
    string? RepoUrl,
    string? DemoUrl,
    bool IsPublic,
    DateTime CreatedAt
);

public record CreateProjectDto(
    string Title,
    string Slug,
    string? ShortDesc,
    string? LongDesc,
    string? TechTags,
    string? ThumbnailPath,
    string? RepoUrl,
    string? DemoUrl,
    bool IsPublic
);

public record UpdateProjectDto(
    string Title,
    string Slug,
    string? ShortDesc,
    string? LongDesc,
    string? TechTags,
    string? ThumbnailPath,
    string? RepoUrl,
    string? DemoUrl,
    bool IsPublic
);
