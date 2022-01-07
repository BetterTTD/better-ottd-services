using OpenTTD.SignalrHub.Messages.Models;

namespace OpenTTD.SignalrHub.Messages.Servers;

public record OnServers(Server[] Servers);

public record OnServer(Server Server);

public record OnServerInfoUpdated(Guid ServerId, ServerInfo Info);

public record OnServerClientUpdated(Guid ServerId, Client Client);