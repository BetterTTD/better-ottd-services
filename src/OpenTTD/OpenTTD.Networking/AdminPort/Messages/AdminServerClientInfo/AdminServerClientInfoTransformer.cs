using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientInfo;

public sealed class AdminServerClientInfoTransformer : IMessageTransformer<AdminServerClientInfoMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_INFO;

    public AdminServerClientInfoMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientInfoMessage
        {
            ClientId = packet.ReadU32(),
            Hostname = packet.ReadString(),
            ClientName = packet.ReadString(),
            Language = packet.ReadByte(),
            JoinDate = packet.ReadU32(),
            PlayingAs = packet.ReadByte(),
        };

        return msg;
    }
}