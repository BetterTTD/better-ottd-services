using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientQuit;

public sealed class AdminServerClientQuitTransformer : IPacketTransformer<AdminServerClientQuit>
{
    public AdminServerClientQuit Transform(Packet packet)
    {
        var msg = new AdminServerClientQuit(packet.ReadU32());
        return msg;
    }
}