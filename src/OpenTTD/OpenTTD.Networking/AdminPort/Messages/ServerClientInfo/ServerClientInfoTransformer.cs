using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientInfo;

public sealed class ServerClientInfoTransformer : IPacketTransformer<ServerClientInfoMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_INFO;

    public ServerClientInfoMessage Transform(Packet packet)
    {
        var msg = new ServerClientInfoMessage
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