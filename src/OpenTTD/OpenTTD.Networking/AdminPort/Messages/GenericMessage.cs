using OpenTTD.Networking.AdminPort.Enums;

namespace OpenTTD.Networking.AdminPort.Messages;

public sealed record GenericMessage : IMessage
{
    public PacketType PacketType { get; init; }
}