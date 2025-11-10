using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RizvePortfolio.Application.DTOs;
using RizvePortfolio.Application.Services;

namespace RizvePortfolio.WebApi.Controllers;

[ApiController]
public class CvController : ControllerBase
{
    private readonly ICvService _cvService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CvController> _logger;

    public CvController(ICvService cvService, IConfiguration configuration, ILogger<CvController> logger)
    {
        _cvService = cvService;
        _configuration = configuration;
        _logger = logger;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("/api/admin/cv/upload")]
    public async Task<ActionResult<CvUploadResultDto>> UploadCv([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null)
        {
            return BadRequest("CV file is required.");
        }

        try
        {
            var uploader = User.Identity?.Name ?? "Admin";
            var version = await _cvService.UploadCvAsync(file, uploader, ct);
            var dto = new CvUploadResultDto(version.Id, version.FileName, version.UploadedAt, version.UploadedBy, version.IsActive);
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload CV.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to upload CV.");
        }
    }

    [HttpGet("/api/cv/download")]
    [AllowAnonymous]
    public async Task<IActionResult> DownloadCv(CancellationToken ct)
    {
        var activeCv = await _cvService.GetActiveCvAsync(ct);
        if (activeCv is null)
        {
            return NotFound();
        }

        if (!System.IO.File.Exists(activeCv.FilePath))
        {
            _logger.LogWarning("CV file missing at path {FilePath}", activeCv.FilePath);
            return NotFound();
        }

        var ipHash = ComputeIpHash(HttpContext);
        if (!string.IsNullOrWhiteSpace(ipHash))
        {
            var userAgent = GetHeader(HttpContext, "User-Agent") ?? string.Empty;
            await _cvService.RecordDownloadAsync(ipHash, userAgent, ct);
        }

        var contentType = "application/octet-stream";
        var downloadName = activeCv.FileName;
        return PhysicalFile(activeCv.FilePath, contentType, downloadName);
    }

    [HttpGet("/api/cv/stats")]
    [AllowAnonymous]
    public async Task<ActionResult<CvStatsDto>> GetStats(CancellationToken ct)
    {
        var totalDownloads = await _cvService.GetTotalDownloadCountAsync(ct);
        var activeCv = await _cvService.GetActiveCvAsync(ct);
        var dto = new CvStatsDto(totalDownloads, activeCv?.FileName, activeCv?.UploadedAt);
        return Ok(dto);
    }

    private string? ComputeIpHash(HttpContext context)
    {
        var salt = _configuration["VisitorTracking:IpHashSalt"];
        if (string.IsNullOrWhiteSpace(salt))
        {
            _logger.LogWarning("IP hash salt is not configured; download tracking will be skipped.");
            return null;
        }

        var ipAddress = GetClientIp(context);
        if (string.IsNullOrWhiteSpace(ipAddress))
        {
            return null;
        }

        var material = string.Concat(ipAddress, ":", salt);
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(bytes);
    }

    private static string? GetClientIp(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var forwarded))
        {
            var ip = forwarded.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(ip))
            {
                return ip;
            }
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    private static string? GetHeader(HttpContext context, string name)
    {
        if (context.Request.Headers.TryGetValue(name, out var values))
        {
            var value = values.ToString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        return null;
    }
}
