using OpenTTD.AdminClient.Networking.Enums;

namespace OpenTTD.AdminClient.Networking.Messages.Inbound;

public sealed record GenericMessage : IMessage
{
    public PacketType PacketType { get; init; }
}