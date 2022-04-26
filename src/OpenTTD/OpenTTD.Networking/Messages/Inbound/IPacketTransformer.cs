using System.Diagnostics.Contracts;
using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound;

public interface IPacketTransformer<out TMessage> where TMessage : IMessage
{
    PacketType PacketType { get; }
    
    [Pure]
    TMessage Transform(Packet packet);
}