namespace OpenTTD.DataAccess;

public sealed class OttdDbConnectionString
{
    public const string Key = nameof(OttdDbConnectionString);
    public string Value { get; set; } = null!;
}