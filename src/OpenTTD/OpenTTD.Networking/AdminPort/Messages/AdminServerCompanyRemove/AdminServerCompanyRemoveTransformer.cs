using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyRemove;

public sealed class AdminServerCompanyRemoveTransformer : IMessageTransformer<AdminServerCompanyRemoveMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_REMOVE;

    public AdminServerCompanyRemoveMessage Transform(Packet packet)
    {
        var msg = new AdminServerCompanyRemoveMessage(packet.ReadByte(), (AdminCompanyRemoveReason) packet.ReadByte());
        return msg;
    }
}