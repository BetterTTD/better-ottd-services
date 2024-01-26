using CSharpFunctionalExtensions;

namespace OpenTTD.StateService.Domain.ValueObjects;

public sealed class ServerPort : ValueObject<ServerPort>, IComparable<ServerPort>
{
    public int Value { get; private init; }

    public bool IsDefault => Value == 0;
    
    public static ServerPort Default() => new(){Value = default};

    public static ServerPort Create(int value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Port should not be less than 0", nameof(value));
        }

        return new ServerPort { Value = value };
    }
    
    protected override bool EqualsCore(ServerPort other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();

    public int CompareTo(ServerPort? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}