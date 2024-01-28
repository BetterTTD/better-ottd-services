using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using OpenTTD.StateService.DataAccess.Seeder;

namespace OpenTTD.StateService.DataAccess;

public static class ServerModule
{
    public static IServiceCollection AddServerDb(this IServiceCollection services) => services
#if DEBUG
        .AddScoped<IDbSeeder, ServerDbSeeder>()
#endif
        .AddDbContext<ServerContext>()
        .AddDataAccessModule();
}