using OpenTTD.Networking.AdminPort.Messages.Base;

namespace OpenTTD.Networking.AdminPort.Messages.AdminServerClientUpdate;

public sealed class AdminServerClientUpdateTransformer : IPacketTransformer<AdminServerClientUpdateMessage>
{
    public AdminServerClientUpdateMessage Transform(Packet packet)
    {
        var msg = new AdminServerClientUpdateMessage
        {
            ClientId = packet.ReadU32(),
            ClientName = packet.ReadString(),
            CompanyId = packet.ReadByte()
        };
        
        return msg;
    }
}