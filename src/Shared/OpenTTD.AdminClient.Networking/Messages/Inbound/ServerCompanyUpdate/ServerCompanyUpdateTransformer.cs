using OpenTTD.AdminClient.Networking.Common;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerCompanyUpdate;

public sealed class ServerCompanyUpdateTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_COMPANY_UPDATE;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerCompanyUpdateMessage
        {
            CompanyId = packet.ReadByte(),
            CompanyName = packet.ReadString(),
            ManagerName = packet.ReadString(),
            Color = (Color)packet.ReadByte(),
            HasPassword = packet.ReadBool(),
            MonthsOfBankruptcy = packet.ReadByte()
        };

        for (var i = 0; i < msg.ShareOwnersIds.Length; ++i)
            msg.ShareOwnersIds[i] = packet.ReadByte();

        return msg;
    }
}