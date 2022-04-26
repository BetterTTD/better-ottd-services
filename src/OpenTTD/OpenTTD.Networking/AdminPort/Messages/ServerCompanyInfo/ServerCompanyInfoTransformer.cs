using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerCompanyInfo;

public sealed class ServerCompanyInfoTransformer : IPacketTransformer<ServerCompanyInfoMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_INFO;

    public ServerCompanyInfoMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyInfoMessage
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