using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyUpdate;

public sealed class AdminServerCompanyUpdateTransformer : IMessageTransformer<AdminServerCompanyUpdateMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_UPDATE;

    public AdminServerCompanyUpdateMessage Transform(Packet packet)
    {
        var msg = new AdminServerCompanyUpdateMessage
        {
            CompanyId = packet.ReadByte(),
            CompanyName = packet.ReadString(),
            ManagerName = packet.ReadString(),
            Color = packet.ReadByte(),
            HasPassword = packet.ReadBool(),
            MonthsOfBankruptcy = packet.ReadByte()
        };
        
        for (var i = 0; i < msg.ShareOwnersIds.Length; ++i) 
            msg.ShareOwnersIds[i] = packet.ReadByte();

        return msg;
    }
}