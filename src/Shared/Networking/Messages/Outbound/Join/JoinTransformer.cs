using Networking.Common;
using Networking.Enums;

namespace Networking.Messages.Outbound.Join;

public sealed class JoinTransformer : IMessageTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_JOIN;

    public Packet Transform(IMessage message)
    {
        var msg = (message as JoinMessage)!;
        var packet = new Packet();

        packet.SendByte((byte)msg.PacketType);
        packet.SendString(msg.Password, 33);
        packet.SendString(msg.AdminName, 25);
        packet.SendString(msg.AdminVersion, 33);

        return packet;
    }
}