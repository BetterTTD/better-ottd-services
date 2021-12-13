using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class GenericAdminMessage : IAdminMessage
{
    public AdminMessageType MessageType { get; }

    public GenericAdminMessage(AdminMessageType type) => MessageType = type;
}