using EventBus.Events;
using OpenTTD.AdminClientDomain.ValueObjects;
using OpenTTD.Networking.Enums;

namespace IntegrationEvents;

public record TestEvent : IntegrationEvent
{
    public ServerId ServerId { get; set; }
    public PacketType Type { get; set; }
    public string Message { get; set; } 
}