using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientQuit;

public sealed class AdminServerClientQuitTransformer : IMessageTransformer<AdminServerClientQuit>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CLIENT_QUIT;

    public AdminServerClientQuit Transform(Packet packet)
    {
        var msg = new AdminServerClientQuit(packet.ReadU32());
        return msg;
    }
}