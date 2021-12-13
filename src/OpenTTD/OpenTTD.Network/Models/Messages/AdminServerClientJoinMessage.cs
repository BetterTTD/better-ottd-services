using OpenTTD.Network.Models.Enums;

namespace OpenTTD.Network.Models.Messages;

public class AdminServerClientJoinMessage : IAdminMessage
{
    public AdminMessageType MessageType => AdminMessageType.AdminPacketServerClientJoin;

    public uint ClientId { get; }

    public AdminServerClientJoinMessage(uint clientId) => ClientId = clientId;
}