using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public interface IAdminMessage
{
    AdminMessageType MessageType { get; }
}