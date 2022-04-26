using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.Join;

public sealed class JoinTransformer : IMessageTransformer<JoinMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_JOIN;

    public Packet Transform(JoinMessage msg)
    {
        var packet = new Packet();

        packet.SendByte((byte)msg.PacketType);
        packet.SendString(msg.Password, 33);
        packet.SendString(msg.AdminName, 25);
        packet.SendString(msg.AdminVersion, 33);

        return packet;
    }
}