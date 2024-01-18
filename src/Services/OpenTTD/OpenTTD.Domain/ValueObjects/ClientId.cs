using CSharpFunctionalExtensions;

namespace OpenTTD.Domain.ValueObjects;

public sealed class ClientId : ValueObject<ClientId>, IComparable<ClientId>
{
    private const uint AdminClientId = 1;

    public uint Value { get; private init; }

    public bool IsAdminClientId => Value == AdminClientId;

    private ClientId()
    {
    }
    
    public static ClientId Create(uint value)
    {
        if (value is > 255 or <= AdminClientId)
        {
            throw new ArgumentException(
                $"Expected value for ClientId should be in range between 2 and 255. Provided value is {value}");
        }

        return new ClientId { Value = value };
    }
    
    public static ClientId CreateForAdmin() => new() {Value = AdminClientId};

    public int CompareTo(ClientId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
    
    protected override bool EqualsCore(ClientId other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();
}