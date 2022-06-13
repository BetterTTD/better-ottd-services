using System.Diagnostics.Contracts;
using Networking.Common;
using Networking.Enums;

namespace Networking.Messages.Inbound;

public interface IPacketTransformer
{
    PacketType PacketType { get; }
    
    [Pure]
    IMessage Transform(Packet packet);
}