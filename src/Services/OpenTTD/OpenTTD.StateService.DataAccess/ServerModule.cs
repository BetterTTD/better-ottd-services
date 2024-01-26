using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.StateService.DataAccess;

public static class ServerModule
{
    public static IServiceCollection AddServerDb(this IServiceCollection services) =>
        services.AddDbContext<ServerContext>();
}