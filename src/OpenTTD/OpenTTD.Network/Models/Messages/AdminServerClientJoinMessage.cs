using OpenTTD.Network.Enums;

namespace OpenTTD.Network.AdminPort.Messages;

public class AdminServerClientJoinMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientJoin;

    public uint ClientId { get; }

    public AdminServerClientJoinMessage(uint clientId) => ClientId = clientId;
}