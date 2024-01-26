using CSharpFunctionalExtensions;

namespace OpenTTD.StateService.Domain.ValueObjects;

public sealed class ServerName : ValueObject<ServerName>, IComparable<ServerName>
{
    private ServerName(string value) => Value = value;

    public string Value { get; }

    public static ServerName Default() => new(string.Empty);

    public static ServerName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Server name should have some value", nameof(value));
        }
        
        return new ServerName(value);
    }

    protected override bool EqualsCore(ServerName other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();

    public int CompareTo(ServerName? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(Value, other.Value, StringComparison.Ordinal);
    }
}