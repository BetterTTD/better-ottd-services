namespace OpenTTD.DataAccess.Models.Base;

public abstract class DataEntity<TId>
{
    public TId Id { get; set; } = default!;
}