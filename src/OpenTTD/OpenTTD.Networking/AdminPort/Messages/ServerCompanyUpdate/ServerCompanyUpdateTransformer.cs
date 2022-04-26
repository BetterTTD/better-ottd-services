using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerCompanyUpdate;

public sealed class ServerCompanyUpdateTransformer : IPacketTransformer<ServerCompanyUpdateMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_UPDATE;

    public ServerCompanyUpdateMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyUpdateMessage
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