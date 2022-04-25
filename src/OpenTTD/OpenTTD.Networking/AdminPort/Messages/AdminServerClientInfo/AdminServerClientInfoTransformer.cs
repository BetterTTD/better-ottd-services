using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientInfo;

public sealed class AdminServerClientInfoTransformer : IPacketTransformer<AdminServerClientInfoMessage>
{
    public AdminServerClientInfoMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientInfoMessage
        {
            ClientId = packet.ReadU32(),
            Hostname = packet.ReadString(),
            ClientName = packet.ReadString(),
            Language = packet.ReadByte(),
            JoinDate = packet.ReadU32(),
            PlayingAs = packet.ReadByte(),
        };

        return msg;
    }
}