using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyNew;

public sealed class AdminServerCompanyNewTransformer : IMessageTransformer<AdminServerCompanyNewMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_NEW;

    public AdminServerCompanyNewMessage Transform(Packet packet)
    {
        var msg = new AdminServerCompanyNewMessage(packet.ReadByte());
        return msg;
    }
}