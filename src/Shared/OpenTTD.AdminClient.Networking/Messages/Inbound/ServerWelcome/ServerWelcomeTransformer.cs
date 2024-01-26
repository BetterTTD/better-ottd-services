using OpenTTD.AdminClient.Networking.Common;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerWelcome;

public sealed class ServerWelcomeTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_WELCOME;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerWelcomeMessage
        {
            ServerName = packet.ReadString(),
            NetworkRevision = packet.ReadString(),
            IsDedicated = packet.ReadBool(),
            MapName = packet.ReadString(),
            MapSeed = packet.ReadU32(),
            Landscape = (Landscape)packet.ReadByte(),
            CurrentDate = packet.ReadU32(),
            MapWidth = packet.ReadU16(),
            MapHeight = packet.ReadU16(),
        };
        return msg;
    }
}