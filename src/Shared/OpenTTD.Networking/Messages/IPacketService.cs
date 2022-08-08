using System.Diagnostics.Contracts;
using OpenTTD.Networking.Common;

namespace OpenTTD.Networking.Messages;

public interface IPacketService
{
    [Pure]
    IMessage ReadPacket(Packet packet);
    
    [Pure]
    Packet CreatePacket(IMessage message);
}