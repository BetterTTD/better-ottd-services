using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerPong;

public sealed record ServerPongMessage(uint Argument) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PONG;
}