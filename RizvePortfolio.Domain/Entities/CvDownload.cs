namespace RizvePortfolio.Domain.Entities;

public class CvDownload
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CvVersionId { get; set; }
    public Guid VisitorId { get; set; }
    public string IpHash { get; set; } = string.Empty;
    public string? UserAgent { get; set; }
    public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public CvVersion? CvVersion { get; set; }
    public Visitor? Visitor { get; set; }
}
