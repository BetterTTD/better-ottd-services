namespace DataAccess.Abstractions;

public abstract class Entity<TId>
{
    public TId Id { get; set; } = default!;
}