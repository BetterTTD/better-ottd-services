using EventBus.Events;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace IntegrationEvents;

public sealed record ServerErrorEvent : IntegrationEvent
{
    public required ServerId ServerId { get; set; }
    public required Exception Exception { get; set; }
    public required string Message { get; set; } 
}