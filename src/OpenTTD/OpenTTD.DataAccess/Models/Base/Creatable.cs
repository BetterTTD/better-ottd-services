namespace OpenTTD.DataAccess.Models.Base;

public interface ICreatable
{
    public DateTime Created { get; set; }
}

public abstract class Creatable<TId> : DataEntity<TId>, ICreatable
{
    public DateTime Created { get; set; }
}