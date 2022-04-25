using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientUpdate;

public sealed class AdminServerClientUpdateTransformer : IMessageTransformer<AdminServerClientUpdateMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;

    public AdminServerClientUpdateMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientUpdateMessage
        {
            ClientId = packet.ReadU32(),
            ClientName = packet.ReadString(),
            CompanyId = packet.ReadByte()
        };
        
        return msg;
    }
}