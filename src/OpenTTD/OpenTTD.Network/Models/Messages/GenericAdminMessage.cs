using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class GenericAdminMessage : IAdminMessage
{
    public AdminMessageType MessageType { get; }

    public GenericAdminMessage(AdminMessageType type) => MessageType = type;
}