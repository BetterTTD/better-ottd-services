using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerPong;

public sealed class AdminServerPongTransformer : IMessageTransformer<AdminServerPongMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_PONG;

    public AdminServerPongMessage Transform(Packet packet)
    {
        var msg = new AdminServerPongMessage(packet.ReadU32());
        return msg;
    }
}