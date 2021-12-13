using System.Collections.Concurrent;
using OpenTTD.Network.AdminPort.Events;
using OpenTTD.Network.AdminPort.Messages;
using OpenTTD.Network.Enums;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort;

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