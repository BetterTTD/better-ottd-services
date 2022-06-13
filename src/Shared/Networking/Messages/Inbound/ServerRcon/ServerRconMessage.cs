using Networking.Enums;

namespace Networking.Messages.Inbound.ServerRcon;

public sealed record ServerRconMessage(ushort Color, string Result) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_RCON;
}