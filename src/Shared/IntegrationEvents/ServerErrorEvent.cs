using EventBus.Events;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace IntegrationEvents;

public sealed record ServerErrorEvent : IntegrationEvent
{
    public required ServerId ServerId { get; set; }
    public required Exception Exception { get; set; }
    public required string Message { get; set; } 
}