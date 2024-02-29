using System.Diagnostics.Contracts;

namespace OpenTTD.AdminClient.Networking.Messages;

public interface IPacketService
{
    [Pure]
    IMessage ReadPacket(Packet packet);
    
    [Pure]
    Packet CreatePacket(IMessage message);
}