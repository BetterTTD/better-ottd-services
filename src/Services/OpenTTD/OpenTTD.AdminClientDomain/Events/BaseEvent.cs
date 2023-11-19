using MediatR;

namespace OpenTTD.AdminClientDomain.Events;

public abstract record BaseEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime TimeStamp { get; } = DateTime.Now;
}