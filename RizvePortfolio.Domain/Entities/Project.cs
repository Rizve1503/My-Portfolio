namespace RizvePortfolio.Domain.Entities;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDesc { get; set; }
    public string? LongDesc { get; set; }
    public string? TechTags { get; set; } // JSON array stored as string
    public string? ThumbnailPath { get; set; }
    public string? RepoUrl { get; set; }
    public string? DemoUrl { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
