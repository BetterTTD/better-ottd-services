using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientJoin;

public sealed class AdminServerClientJoinTransformer : IPacketTransformer<AdminServerClientJoinMessage>
{
    public AdminServerClientJoinMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientJoinMessage(packet.ReadU32());
        return msg;
    }
}