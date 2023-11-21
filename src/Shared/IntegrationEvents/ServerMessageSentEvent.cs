using EventBus.Events;
using OpenTTD.AdminClientDomain.ValueObjects;
using OpenTTD.Networking.Enums;

namespace IntegrationEvents;

public sealed record ServerMessageSentEvent : IntegrationEvent
{
    public required ServerId ServerId { get; set; }
    public required PacketType Type { get; set; }
    public required string Message { get; set; } 
}