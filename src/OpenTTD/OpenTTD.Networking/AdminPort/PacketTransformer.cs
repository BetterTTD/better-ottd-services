using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort;

public sealed class PacketTransformer : IPacketTransformer
{
    private readonly List<IMessageTransformer<IAdminMessage>> _transformers;

    public PacketTransformer(List<IMessageTransformer<IAdminMessage>> transformers)
    {
        _transformers = transformers;
    }

    public IAdminMessage Transform(Packet packet)
    {
        var type = packet.ReadByte();

        return Enum.IsDefined(typeof(AdminPacketType), type)
            ? TransformPacket((AdminPacketType)type, packet)
            : new GenericAdminMessage { PacketType = AdminPacketType.INVALID_ADMIN_PACKET };
    }

    private IAdminMessage TransformPacket(AdminPacketType packetType, Packet packet)
    {
        var transformer = _transformers.FirstOrDefault(tr => tr.PacketType == packetType);

        return transformer is not null
            ? transformer.Transform(packet)
            : new GenericAdminMessage { PacketType = packetType };
    }
}