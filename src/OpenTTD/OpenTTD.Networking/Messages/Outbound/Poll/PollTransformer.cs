using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.Poll;

public sealed class PollTransformer : IMessageTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_POLL;
    
    public Packet Transform(IMessage message)
    {
        var msg = (message as PollMessage)!;
        var packet = new Packet();
        
        packet.SendByte((byte)msg.PacketType);
        
        packet.SendByte((byte)msg.UpdateType);
        packet.SendU32(msg.Argument);

        return packet;
    }
}