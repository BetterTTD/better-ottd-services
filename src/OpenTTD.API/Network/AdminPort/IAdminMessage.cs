namespace OpenTTD.API.Network.AdminPort;

public interface IAdminMessage
{
    AdminMessageType MessageType { get; }
}