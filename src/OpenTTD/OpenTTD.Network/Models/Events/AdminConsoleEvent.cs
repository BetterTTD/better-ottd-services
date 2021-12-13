using OpenTTD.Network.Enums;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort.Events;

public class AdminConsoleEvent : IAdminEvent
{
    public string Origin { get; }

    public string Message { get; }

    public AdminEventType EventType => AdminEventType.ConsoleMessage;
    public ServerInfo Server { get; }

    public AdminConsoleEvent(ServerInfo server, string origin, string message)
    {
        Origin = origin;
        Message = message;
        Server = server;
    }
        
}