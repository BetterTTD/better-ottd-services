using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

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