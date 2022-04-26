using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.Rcon;

public record RconMessage(string Command) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_RCON;
}

public sealed class RconTransformer : IMessageTransformer<RconMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_RCON;
    
    public Packet Transform(RconMessage msg)
    {
        var packet = new Packet();

        packet.SendByte((byte)msg.PacketType);

        packet.SendString(msg.Command, 500);

        return packet;
    }
}