using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.DataAccess;

public static class Module
{
    public static IServiceCollection AddDataAccessModule(this IServiceCollection services, IConfiguration cfg)
    {
        services.Configure<AdminClientConnectionString>(connStr =>
        {
            var value = cfg.GetConnectionString(nameof(AdminClientConnectionString));
            
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("Connection string should not be empty");
            }

            connStr.Value = value;
        });
            
        services.AddDbContext<AdminClientContext>();
        
        return services;
    }
}