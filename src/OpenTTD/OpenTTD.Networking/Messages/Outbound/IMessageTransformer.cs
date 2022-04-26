using System.Diagnostics.Contracts;
using OpenTTD.Networking.Common;
using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Outbound;

public interface IMessageTransformer
{
    PacketType PacketType { get; }
    
    [Pure]
    Packet Transform(IMessage msg);   
}