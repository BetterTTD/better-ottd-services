using System.Diagnostics.Contracts;
using OpenTTD.AdminClient.Networking.Common;
using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Outbound;

public interface IMessageTransformer
{
    PacketType PacketType { get; }
    
    [Pure]
    Packet Transform(IMessage msg);   
}