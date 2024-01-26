using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerPong;

public sealed record ServerPongMessage(uint Argument) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_PONG;
}