namespace OpenTTD.API.Network.AdminPort;

public class GenericAdminMessage : IAdminMessage
{
    public AdminMessageType MessageType { get; }

    public GenericAdminMessage(AdminMessageType type) => MessageType = type;
}