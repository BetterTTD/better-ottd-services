using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Outbound.Rcon;

public record RconMessage(string Command) : IMessage
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_RCON;
}

public sealed class RconTransformer : IMessageTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_RCON;
    
    public Packet Transform(IMessage message)
    {
        var msg = (message as RconMessage)!;
        var packet = new Packet();

        packet.SendByte((byte)msg.PacketType);

        packet.SendString(msg.Command, 500);

        return packet;
    }
}