using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.Services;

public static class Module
{
    public static IServiceCollection AddServicesModule(this IServiceCollection services)
    {
        services.AddScoped<IServerConfigurationService, ServerConfigurationService>();
        
        return services;
    }
}