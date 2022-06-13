using Networking.Enums;

namespace Networking.Messages.Inbound.ServerClientJoin;

public sealed record ServerClientJoinMessage(uint ClientId) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_JOIN;
}