using System.Diagnostics.Contracts;
using OpenTTD.AdminClient.Networking.Common;

namespace OpenTTD.AdminClient.Networking.Messages;

public interface IPacketService
{
    [Pure]
    IMessage ReadPacket(Packet packet);
    
    [Pure]
    Packet CreatePacket(IMessage message);
}