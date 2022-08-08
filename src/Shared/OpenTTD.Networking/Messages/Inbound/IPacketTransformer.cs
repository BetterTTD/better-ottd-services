using System.Diagnostics.Contracts;
using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound;

public interface IPacketTransformer
{
    PacketType PacketType { get; }
    
    [Pure]
    IMessage Transform(Packet packet);
}