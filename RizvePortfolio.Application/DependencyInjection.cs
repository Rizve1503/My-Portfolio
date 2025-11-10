using Microsoft.Extensions.DependencyInjection;
using RizvePortfolio.Application.Services;

namespace RizvePortfolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPortfolioService, PortfolioService>();
        return services;
    }
}
