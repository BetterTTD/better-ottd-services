using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort;

public interface IAdminMessage
{
    AdminMessageType MessageType { get; }
}