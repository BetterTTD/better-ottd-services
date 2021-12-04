namespace OpenTTD.API.Network.AdminPort;

public class AdminServerClientErrorMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientError;

    public uint ClientId { get; }

    public NetworkErrorCode Error { get; }

    public AdminServerClientErrorMessage(uint clientId, byte error)
    {
        ClientId = clientId;
        Error = (NetworkErrorCode)error;
    }
}