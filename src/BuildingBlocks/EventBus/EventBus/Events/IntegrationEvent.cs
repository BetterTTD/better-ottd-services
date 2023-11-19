namespace EventBus.Events;

public abstract record IntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime CreationDate { get; } = DateTime.UtcNow;
}