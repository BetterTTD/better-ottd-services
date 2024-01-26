using OpenTTD.AdminClient.Networking.Common;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound.ServerRcon;

public sealed class ServerRconTransformer : IPacketTransformer
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_RCON;

    public IMessage Transform(Packet packet)
    {
        var msg = new ServerRconMessage(packet.ReadU16(), packet.ReadString());
        return msg;
    }
}