using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Events;

public class AdminPongEvent : IAdminEvent
{
    public AdminEventType EventType => AdminEventType.Pong;

    public ServerInfo Server { get; }

    public uint PongValue { get; }

    public AdminPongEvent(ServerInfo server, uint pongValue)
    {
        Server = server;
        PongValue = pongValue;
    }
}