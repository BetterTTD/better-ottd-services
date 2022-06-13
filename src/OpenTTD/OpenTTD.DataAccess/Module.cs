using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.DataAccess;

public static class Module
{
    public static IServiceCollection AddDataAccessModule(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var cfg = sp.GetRequiredService<IConfiguration>();
            var str = cfg.GetConnectionString(nameof(AdminClientConnectionString));

            return string.IsNullOrWhiteSpace(str)
                ? throw new ArgumentException("Connection string can not be null or empty", nameof(str))
                : new AdminClientConnectionString(str);
        });
            
        services.AddDbContext<AdminClientContext>();
        
        return services;
    }
}