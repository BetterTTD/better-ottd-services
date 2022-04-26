using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages.ServerClientUpdate;

public sealed class ServerClientUpdateTransformer : IPacketTransformer<ServerClientUpdateMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;

    public ServerClientUpdateMessage Transform(Packet packet)
    {
        var msg = new ServerClientUpdateMessage
        {
            ClientId = packet.ReadU32(),
            ClientName = packet.ReadString(),
            CompanyId = packet.ReadByte()
        };
        
        return msg;
    }
}