using EventBus.Events;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Networking.Enums;

namespace IntegrationEvents;

public sealed record ServerMessageSentEvent : IntegrationEvent
{
    public required ServerId ServerId { get; set; }
    public required PacketType Type { get; set; }
    public required string Message { get; set; } 
}