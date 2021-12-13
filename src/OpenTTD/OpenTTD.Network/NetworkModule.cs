using Microsoft.Extensions.DependencyInjection;
using OpenTTD.Network.AdminClient;
using OpenTTD.Network.AdminMappers;

namespace OpenTTD.Network;

public static class NetworkModule
{
    public static IServiceCollection AddNetworkModule(this IServiceCollection services)
    {
        services.AddSingleton<IAdminPacketService>(new AdminPacketService());
        services.AddSingleton<IAdminPortClientFactory, AdminPortClientFactory>();
        services.AddSingleton<IAdminMessageProcessor, AdminMessageProcessor>();
        
        return services;
    }
}