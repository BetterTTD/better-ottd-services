using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages;

public interface IMessage
{
    PacketType PacketType { get; }
}