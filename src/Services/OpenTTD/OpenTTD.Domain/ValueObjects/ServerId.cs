using CSharpFunctionalExtensions;

namespace OpenTTD.Domain.ValueObjects;

public sealed class ServerId(Guid value) : ValueObject<ServerId>, IComparable<ServerId>
{
    public Guid Value { get; } = value;
    
    protected override bool EqualsCore(ServerId other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();

    public int CompareTo(ServerId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}