using Microsoft.AspNetCore.SignalR;
using OpenTTD.SignalrHub.Messages.Models;
using OpenTTD.SignalrHub.Messages.Servers;

namespace OpenTTD.SignalrHub.Hubs;

public interface IServerHubClient
{
    Task AskServers();
    Task AskServer(Guid serverId);
    Task OnServers(OnServers @event);
    Task OnServer(OnServer @event);
    Task OnServerInfoUpdated(@OnServerInfoUpdated @event);
    Task OnServerClientUpdated(OnServerClientUpdated @event);
}

public class ServerHub : Hub<IServerHubClient>
{
    private readonly ILogger<ServerHub> _logger;

    public ServerHub(ILoggerFactory factory) => 
        _logger = factory.CreateLogger<ServerHub>();

    public async Task AskServers()
    {
        _logger.LogInformation("AskServers");
        await Clients.Others.AskServers();
    }

    public async Task AskServer(Guid serverId)
    {
        _logger.LogInformation("AskServer");
        await Clients.Others.AskServer(serverId);
    }

    public async Task TellServers(Server[] servers)
    {
        _logger.LogInformation("TellServers");
        await Clients.Others.OnServers(new OnServers(servers));
    }

    public async Task TellServer(Server server)
    {
        _logger.LogInformation("TellServer");
        await Clients.Others.OnServer(new OnServer(server));
    }

    public async Task TellServerInfoUpdated(Guid serverId, ServerInfo info)
    {
        _logger.LogInformation("TellServerInfoUpdated");
        await Clients.Others.OnServerInfoUpdated(new OnServerInfoUpdated(serverId, info));
    }

    public async Task TellServerClientUpdated(Guid serverId, Client client)
    {
        _logger.LogInformation("TellServerClientUpdated");
        await Clients.Others.OnServerClientUpdated(new OnServerClientUpdated(serverId, client));
    }
}