using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound.ServerRcon;

public sealed class ServerRconTransformer : IPacketTransformer<ServerRconMessage>
{
    public PacketType PacketType => PacketType.ADMIN_PACKET_SERVER_RCON;

    public ServerRconMessage Transform(Packet packet)
    {
        var msg = new ServerRconMessage(packet.ReadU16(), packet.ReadString());
        return msg;
    }
}