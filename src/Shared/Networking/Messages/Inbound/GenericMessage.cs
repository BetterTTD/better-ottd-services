using Networking.Enums;

namespace Networking.Messages.Inbound;

public sealed record GenericMessage : IMessage
{
    public PacketType PacketType { get; init; }
}