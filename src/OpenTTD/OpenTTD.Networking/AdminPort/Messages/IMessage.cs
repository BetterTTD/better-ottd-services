using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public interface IMessage
{
    PacketType PacketType { get; }
}