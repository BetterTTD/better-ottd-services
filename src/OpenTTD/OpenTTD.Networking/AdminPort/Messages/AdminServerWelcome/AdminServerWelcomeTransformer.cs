using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerWelcome;

public sealed class AdminServerWelcomeMessageTransformer : IMessageTransformer<AdminServerWelcomeMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_WELCOME;

    public AdminServerWelcomeMessage Transform(Packet packet)
    {
        var msg = new AdminServerWelcomeMessage
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