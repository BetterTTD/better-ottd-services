using System.Diagnostics.Contracts;
using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public interface IPacketTransformer<out TMessage> where TMessage : IMessage
{
    PacketType PacketType { get; }
    
    [Pure]
    TMessage Transform(Packet packet);
}