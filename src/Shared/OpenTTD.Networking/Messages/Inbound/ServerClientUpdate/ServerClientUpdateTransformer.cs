using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerClientUpdate;

public sealed class ServerClientUpdateTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_CLIENT_UPDATE;

    public IMessage Transform(Packet packet)
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