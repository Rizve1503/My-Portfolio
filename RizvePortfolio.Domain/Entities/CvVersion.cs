namespace RizvePortfolio.Domain.Entities;

public class CvVersion
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    public string UploadedBy { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    // Navigation
    public ICollection<CvDownload> CvDownloads { get; set; } = new List<CvDownload>();
}
