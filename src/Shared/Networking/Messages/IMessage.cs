using Networking.Enums;

namespace Networking.Messages;

public interface IMessage
{
    PacketType PacketType { get; }
}