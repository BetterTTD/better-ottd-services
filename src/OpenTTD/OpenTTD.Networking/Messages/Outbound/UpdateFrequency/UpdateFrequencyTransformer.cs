using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound.UpdateFrequency;

public sealed class UpdateFrequencyTransformer : IMessageTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_ADMIN_UPDATE_FREQUENCY;

    public Packet Transform(IMessage message)
    {
        var msg = (message as UpdateFrequencyMessage)!;
        var packet = new Packet();

        packet.SendByte((byte)msg.PacketType);

        packet.SendU16((ushort)msg.UpdateType);
        packet.SendU16((ushort)msg.UpdateFrequency);

        return packet;
    }
}