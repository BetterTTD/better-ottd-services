namespace OpenTTD.API.Network.AdminPort;

public interface IAdminEvent
{
    AdminEventType EventType { get; }

    ServerInfo Server { get; }
}