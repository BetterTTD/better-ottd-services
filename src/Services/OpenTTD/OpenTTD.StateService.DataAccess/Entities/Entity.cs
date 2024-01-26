namespace OpenTTD.StateService.DataAccess.Entities;

public abstract class Entity<TId>
{
    public required TId Id { get; set; }
}