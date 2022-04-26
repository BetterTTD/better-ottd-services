using System.Diagnostics.Contracts;
using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public interface IMessageTransformer<in TMessage> where  TMessage : IMessage
{
    PacketType PacketType { get; }
    
    [Pure]
    Packet Transform(TMessage message);   
}