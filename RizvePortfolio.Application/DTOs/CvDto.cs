namespace RizvePortfolio.Application.DTOs;

public record CvVersionDto(
    Guid Id,
    string FileName,
    string FilePath,
    string? UploadedBy,
    DateTime UploadedAt,
    bool IsActive
);

public record CvDownloadDto(
    Guid Id,
    Guid CvVersionId,
    string IpHash,
    DateTime DownloadedAt
);

public record VisitorDto(
    Guid VisitorId,
    string IpHash,
    string Path,
    string? Referrer,
    string? UserAgent,
    DateTime CreatedAt
);
