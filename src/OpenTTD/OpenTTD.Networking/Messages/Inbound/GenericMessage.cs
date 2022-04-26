using OpenTTD.Networking.Enums;

namespace OpenTTD.Networking.Messages.Inbound;

public sealed record GenericMessage : IMessage
{
    public PacketType PacketType { get; init; }
}