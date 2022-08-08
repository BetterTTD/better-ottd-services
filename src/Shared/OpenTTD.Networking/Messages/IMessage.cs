using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages;

public interface IMessage
{
    PacketType PacketType { get; }
}