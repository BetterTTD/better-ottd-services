namespace DataAccess.Abstractions;

public abstract class Entity<TId>
{
    public required TId Id { get; set; }
}