using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RizvePortfolio.Application.Abstractions;
using RizvePortfolio.Application.Services;
using RizvePortfolio.Infrastructure.Identity;
using RizvePortfolio.Infrastructure.Persistence;
using RizvePortfolio.Infrastructure.Services;

namespace RizvePortfolio.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? configuration.GetConnectionString("Default") 
            ?? "Data Source=RizvePortfolio.db";

        // Determine which provider to use based on connection string
        if (connectionString.Contains("Data Source=") && connectionString.EndsWith(".db"))
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));
        }
        else
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));
        }

        // Identity configuration
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();

        services.AddHttpContextAccessor();

        // Repository registrations
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IVisitorRepository, VisitorRepository>();
        services.AddScoped<ICvRepository, CvRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();

        // File storage
        services.AddSingleton<IFileStorage, FileStorageService>();
            services.AddScoped<ICvService, CvService>();
            services.AddScoped<IVisitorService, VisitorService>();

        return services;
    }
}

