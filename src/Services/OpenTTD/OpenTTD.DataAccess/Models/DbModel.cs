namespace OpenTTD.DataAccess.Models;

public class DbModel<TKey>
{
    public TKey Id { get; protected set; } = default!;
}