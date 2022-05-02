namespace OpenTTD.Domain.Entities;

public abstract record Entity<TKey>
{
    public TKey Id { get; init; } = default!;
}