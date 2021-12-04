namespace OpenTTD.API.Network.AdminPort;

public class AdminServerClientJoinMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientJoin;

    public uint ClientId { get; }

    public AdminServerClientJoinMessage(uint clientId) => ClientId = clientId;
}