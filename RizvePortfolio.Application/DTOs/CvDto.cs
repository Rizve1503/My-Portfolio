namespace RizvePortfolio.Application.DTOs;

public record CvVersionDto(
    Guid Id,
    string FileName,
    string FilePath,
    string UploadedBy,
    DateTime UploadedAt,
    bool IsActive
);

public record CvDownloadDto(
    Guid Id,
    Guid CvVersionId,
    string IpHash,
    string? UserAgent,
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

public record CvUploadResultDto(
    Guid Id,
    string FileName,
    DateTime UploadedAt,
    string UploadedBy,
    bool IsActive
);

public record CvStatsDto(
    int TotalDownloads,
    string? ActiveFileName,
    DateTime? ActiveUploadedAt
);
