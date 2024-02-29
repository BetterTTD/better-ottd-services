using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Outbound.Ping;

public sealed class PingTransformer : IMessageTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_PING;
    
    public Packet Transform(IMessage message)
    {
        var msg = (message as PingMessage)!;
        var packet = new Packet();

        packet.SendByte((byte)msg.PacketType);
        packet.SendU32(msg.Argument);

        return packet;
    }
}