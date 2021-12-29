using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Events;

public interface IAdminEvent
{
    AdminEventType EventType { get; }
    ServerInfo Server { get; }
}