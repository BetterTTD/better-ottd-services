using Microsoft.Extensions.DependencyInjection;
using OpenTTD.DataAccess.Seeder;

namespace OpenTTD.DataAccess;

public static class Module
{
    public static IServiceCollection AddOttdDataAccessModule(this IServiceCollection services, string connStr)
    {
        services.Configure<OttdDbConnectionString>(conn => conn.Value = connStr);
        services.AddDbContext<OttdContext>();
        services.AddTransient<OttdDbSeeder>();
        
        return services;
    }
}