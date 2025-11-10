using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RizvePortfolio.Application.Services;

namespace RizvePortfolio.WebApi.Middleware;

public class VisitorTrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IVisitorService _visitorService;
    private readonly ILogger<VisitorTrackingMiddleware> _logger;
    private readonly string? _ipHashSalt;

    public VisitorTrackingMiddleware(
        RequestDelegate next,
        IVisitorService visitorService,
        IConfiguration configuration,
        ILogger<VisitorTrackingMiddleware> logger)
    {
        _next = next;
        _visitorService = visitorService;
        _logger = logger;
        _ipHashSalt = configuration["VisitorTracking:IpHashSalt"];

        if (string.IsNullOrWhiteSpace(_ipHashSalt))
        {
            _logger.LogWarning("Visitor tracking salt is not configured; visits will be skipped.");
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (ShouldTrackRequest(context))
        {
            try
            {
                if (!IsAdmin(context.User))
                {
                    var ipHash = ComputeIpHash(GetClientIp(context));
                    if (!string.IsNullOrWhiteSpace(ipHash))
                    {
                        var path = context.Request.Path.ToString();
                        var referrer = GetHeader(context, "Referer") ?? string.Empty;
                        var userAgent = GetHeader(context, "User-Agent") ?? string.Empty;
                        await _visitorService.TrackVisitAsync(path, referrer, userAgent, ipHash);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to track visitor for path {Path}", context.Request.Path);
            }
        }

        await _next(context);
    }

    private bool ShouldTrackRequest(HttpContext context)
    {
        if (string.IsNullOrWhiteSpace(_ipHashSalt))
        {
            return false;
        }

        var method = context.Request.Method;
        if (!HttpMethods.IsGet(method) && !HttpMethods.IsHead(method))
        {
            return false;
        }

        var path = context.Request.Path;
        if (!path.HasValue)
        {
            return false;
        }

        var value = path.Value!;

        if (value.StartsWith("/api/admin", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (value.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (value.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (value.StartsWith("/uploads", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (HasStaticAssetExtension(value))
        {
            return false;
        }

        return true;
    }

    private static bool HasStaticAssetExtension(string path)
    {
        var extension = System.IO.Path.GetExtension(path);
        if (string.IsNullOrEmpty(extension))
        {
            return false;
        }

        return extension switch
        {
            ".css" or ".js" or ".png" or ".jpg" or ".jpeg" or ".gif" or ".svg" or ".ico" or ".woff" or ".woff2" or ".ttf" or ".map" or ".json" or ".xml" or ".txt" or ".pdf" => true,
            _ => false
        };
    }

    private static bool IsAdmin(System.Security.Claims.ClaimsPrincipal? user)
        => user?.Identity is not null && user.Identity.IsAuthenticated && user.IsInRole("Admin");

    private string? ComputeIpHash(string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrWhiteSpace(_ipHashSalt))
        {
            return null;
        }

        var material = string.Concat(ipAddress, ":", _ipHashSalt);

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

    private static string? GetHeader(HttpContext context, string headerName)
    {
        if (context.Request.Headers.TryGetValue(headerName, out var values))
        {
            var value = values.ToString();
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        return null;
    }
}
