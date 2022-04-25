using OpenTTD.Networking.AdminPort.Enums;
using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerConsole;

public sealed class AdminServerConsoleTransformer : IMessageTransformer<AdminServerConsoleMessage>
{
    public AdminPacketType PacketType => AdminPacketType.ADMIN_PACKET_SERVER_CONSOLE;

    public AdminServerConsoleMessage Transform(Packet packet)
    {
        var msg = new AdminServerConsoleMessage(packet.ReadString(), packet.ReadString());
        return msg;
    }
}