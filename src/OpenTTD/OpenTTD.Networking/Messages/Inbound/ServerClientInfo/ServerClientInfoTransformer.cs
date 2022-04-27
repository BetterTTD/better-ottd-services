using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientInfo;

public sealed class ServerClientInfoTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_INFO;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerClientInfoMessage
        {
            ClientId = packet.ReadU32(),
            Hostname = packet.ReadString(),
            ClientName = packet.ReadString(),
            Language = packet.ReadByte(),
            JoinDate = packet.ReadU32(),
            CompanyId = packet.ReadByte(),
        };

        return msg;
    }
}