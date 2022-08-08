using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientQuit;

public sealed record ServerClientQuitMessage(uint ClientId) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_QUIT;
}