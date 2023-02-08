namespace EventBus.Events;

public abstract class IntegrationEvent
{
    public IntegrationEvent()
    {
        this.Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;           
    }

    public IntegrationEvent(Guid id, DateTime creationDate)
    {
        this.Id = id;
        this.CreationDate = creationDate;
    }

    public Guid Id { get; }
    public DateTime CreationDate { get; }
}