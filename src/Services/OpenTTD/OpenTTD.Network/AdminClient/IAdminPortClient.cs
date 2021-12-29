using System.Collections.Concurrent;
using OpenTTD.Network.Models;
using OpenTTD.Network.Models.Enums;
using OpenTTD.Network.Models.Events;
using OpenTTD.Network.Models.Messages;

namespace OpenTTD.Network.AdminClient;

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