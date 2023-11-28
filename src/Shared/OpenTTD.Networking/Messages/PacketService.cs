using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;
using OpenTTD.Networking.Messages.Inbound;
using OpenTTD.Networking.Messages.Outbound;

namespace OpenTTD.Networking.Messages;

public sealed class PacketService(IEnumerable<IPacketTransformer> packetTransformers,
        IEnumerable<IMessageTransformer> messageTransformers)
    : IPacketService
{
    public IMessage ReadPacket(Packet packet)
    {
        var type = packet.ReadByte();

        return Enum.IsDefined(typeof(PacketType), (int) type)
            ? TransformPacket((PacketType)type, packet)
            : new GenericMessage { PacketType = PacketType.INVALID_ADMIN_PACKET };
    }

    public Packet CreatePacket(IMessage message)
    {
        var transformer = messageTransformers.FirstOrDefault(mt => mt.PacketType == message.PacketType);
        return transformer!.Transform(message);
    }

    private IMessage TransformPacket(PacketType packetType, Packet packet)
    {
        var transformer = packetTransformers.FirstOrDefault(pt => pt.PacketType == packetType);

        return transformer is not null
            ? transformer.Transform(packet)
            : new GenericMessage { PacketType = packetType };
    }
}