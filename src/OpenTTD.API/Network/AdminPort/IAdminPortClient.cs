using System.Collections.Concurrent;

namespace OpenTTD.API.Network.AdminPort;

public interface IAdminPortClient
{
    AdminConnectionState ConnectionState { get; }

    ConcurrentDictionary<AdminUpdateType, AdminUpdateSetting> AdminUpdateSettings { get; }

    ConcurrentDictionary<uint, Player> Players { get; }

    AdminServerInfo AdminServerInfo { get; }

    event EventHandler<IAdminEvent> EventReceived;

    ServerInfo ServerInfo { get; }

    void SendMessage(IAdminMessage message);

    Task Join();
    Task Disconnect();
        
}