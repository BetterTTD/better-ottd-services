using MediatR;

namespace OpenTTD.AdminClient.Domain.Events;

public abstract record BaseEvent : INotification
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime TimeStamp { get; } = DateTime.Now;
}