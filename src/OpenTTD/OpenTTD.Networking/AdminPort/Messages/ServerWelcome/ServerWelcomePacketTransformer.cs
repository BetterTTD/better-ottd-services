using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerWelcome;

public sealed class ServerWelcomePacketTransformer : IPacketTransformer<ServerWelcomeMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_WELCOME;

    public ServerWelcomeMessage Transform(Packet packet)
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