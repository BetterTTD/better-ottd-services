using System.Diagnostics.Contracts;

namespace OpenTTD.Networking.AdminPort.Messages;

public interface IPacketService
{
    [Pure]
    IMessage ReadPacket(Packet packet);
    
    [Pure]
    Packet CreatePacket(IMessage message);
}