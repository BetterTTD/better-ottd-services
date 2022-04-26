using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerCompanyRemove;

public sealed class ServerCompanyRemoveTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyRemoveMessage(packet.ReadByte(), (AdminCompanyRemoveReason) packet.ReadByte());
        return msg;
    }
}