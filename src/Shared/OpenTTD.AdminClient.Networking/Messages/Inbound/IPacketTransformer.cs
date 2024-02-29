using System.Diagnostics.Contracts;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound;

public interface IPacketTransformer
{
    PacketType PacketType { get; }
    
    [Pure]
    IMessage Transform(Packet packet);
}