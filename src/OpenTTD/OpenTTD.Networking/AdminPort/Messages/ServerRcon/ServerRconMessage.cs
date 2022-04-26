using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerRcon;

public sealed record ServerRconMessage(ushort Color, string Result) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_RCON;
}