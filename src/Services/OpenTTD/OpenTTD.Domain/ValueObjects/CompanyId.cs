using CSharpFunctionalExtensions;

namespace OpenTTD.Domain.ValueObjects;

public sealed class CompanyId : ValueObject<CompanyId>, IComparable<CompanyId>
{
    private const uint SpectatorCompanyId = 255;
    
    public bool IsSpectator => Value == SpectatorCompanyId;

    public uint Value { get; init; }

    private CompanyId()
    {
    }

    public static CompanyId Create(uint value)
    {
        if (value is < 1 or >= SpectatorCompanyId)
        {
            throw new ArgumentException(
                $"Expected value for CompanyId should be in range between 1 and 254. Provided value is {value}");
        }

        return new CompanyId { Value = value };
    }
    
    public static CompanyId CreateSpectatorCompanyId() => new() {Value = SpectatorCompanyId}; 

    public int CompareTo(CompanyId? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
    
    protected override bool EqualsCore(CompanyId other) => Value == other.Value;

    protected override int GetHashCodeCore() => Value.GetHashCode();
}