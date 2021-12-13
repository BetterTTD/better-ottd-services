using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public interface IAdminMessage
{
    AdminMessageType MessageType { get; }
}