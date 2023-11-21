using EventBus.Events;
using OpenTTD.AdminClientDomain.Enums;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace IntegrationEvents;

public sealed record ServerStateChangedEvent : IntegrationEvent
{
    public required ServerId ServerId { get; set; }
    public required ServerState FromState { get; set; }
    public required ServerState ToState { get; set; }
}