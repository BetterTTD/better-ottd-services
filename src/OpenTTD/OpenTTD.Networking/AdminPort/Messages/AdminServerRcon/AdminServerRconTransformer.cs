using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerRcon;

public sealed class AdminServerRconTransformer : IMessageTransformer<AdminServerRconMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_RCON;

    public AdminServerRconMessage Transform(Packet packet)
    {
        var msg = new AdminServerRconMessage(packet.ReadU16(), packet.ReadString());
        return msg;
    }
}