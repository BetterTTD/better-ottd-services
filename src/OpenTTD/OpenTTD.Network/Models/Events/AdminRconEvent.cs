using OpenTTD.Network.Enums;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort.Events;

public class AdminRconEvent : IAdminEvent
{
    public AdminEventType EventType => AdminEventType.AdminRcon;

    public ServerInfo Server { get; } 

    public string Message { get; }

    public AdminRconEvent(ServerInfo server, string msg)
    {
        Server = server;
        Message = msg;
    }
}