namespace RizvePortfolio.Domain.Entities;

public class Visitor
{
    public Guid VisitorId { get; set; } = Guid.NewGuid();
    public string IpHash { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<CvDownload> CvDownloads { get; set; } = new List<CvDownload>();
}
