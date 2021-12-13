using OpenTTD.Network.Enums;
using OpenTTD.Network.Models;

namespace OpenTTD.Network.AdminPort.Events;

public interface IAdminEvent
{
    AdminEventType EventType { get; }
    ServerInfo Server { get; }
}