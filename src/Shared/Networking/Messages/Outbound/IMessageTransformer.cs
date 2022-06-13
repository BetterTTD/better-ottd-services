using System.Diagnostics.Contracts;
using Networking.Common;
using Networking.Enums;

namespace Networking.Messages.Outbound;

public interface IMessageTransformer
{
    PacketType PacketType { get; }
    
    [Pure]
    Packet Transform(IMessage msg);   
}