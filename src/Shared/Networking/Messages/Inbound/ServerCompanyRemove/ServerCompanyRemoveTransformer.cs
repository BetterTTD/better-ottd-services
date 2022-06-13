using Networking.Common;
using Networking.Enums;

namespace Networking.Messages.Inbound.ServerCompanyRemove;

public sealed class ServerCompanyRemoveTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyRemoveMessage(packet.ReadByte(), (CompanyRemoveReason) packet.ReadByte());
        return msg;
    }
}