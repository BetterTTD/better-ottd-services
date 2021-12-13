using Microsoft.Extensions.DependencyInjection;

namespace OpenTTD.Network.AdminPort;

public class AdminPortModule
{
    public void Register(in IServiceCollection services)
    {
        services.AddSingleton<IAdminPacketService>(new AdminPacketService());
        services.AddSingleton<IAdminPortClientFactory, AdminPortClientFactory>();
        services.AddSingleton<IAdminMessageProcessor, AdminMessageProcessor>();
    }
}