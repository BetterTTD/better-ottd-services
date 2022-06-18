namespace OpenTTD.DataAccess;

public sealed record AdminClientConnectionString
{
    public string Value { get; set; } = string.Empty;
}