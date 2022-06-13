using System.Diagnostics.Contracts;
using Networking.Common;

namespace Networking.Messages;

public interface IPacketService
{
    [Pure]
    IMessage ReadPacket(Packet packet);
    
    [Pure]
    Packet CreatePacket(IMessage message);
}