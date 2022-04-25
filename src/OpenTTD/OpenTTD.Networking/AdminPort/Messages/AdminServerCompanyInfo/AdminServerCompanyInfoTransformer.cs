using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerCompanyInfo;

public sealed class AdminServerCompanyInfoTransformer : IMessageTransformer<AdminServerCompanyInfoMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_COMPANY_INFO;

    public AdminServerCompanyInfoMessage Transform(Packet packet)
    {
        var msg = new AdminServerCompanyInfoMessage
        {
            CompanyId = packet.ReadByte(),
            CompanyName = packet.ReadString(),
            ManagerName = packet.ReadString(),
            Color = packet.ReadByte(),
            HasPassword = packet.ReadBool(),
            CreationDate = packet.ReadU32(),
            IsAi = packet.ReadBool(),
            MonthsOfBankruptcy = packet.ReadByte()
        };
        
        for (var i = 0; i < msg.ShareOwnersIds.Length; ++i) 
            msg.ShareOwnersIds[i] = packet.ReadByte();

        return msg;
    }
}