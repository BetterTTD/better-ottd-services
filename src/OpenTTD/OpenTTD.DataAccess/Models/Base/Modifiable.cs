namespace OpenTTD.DataAccess.Models.Base;

public interface IModifiable : ICreatable
{
    public DateTime Modified { get; set; }
}

public abstract class Modifiable<TId> : Creatable<TId>, IModifiable
{
    public DateTime Modified { get; set; }
}