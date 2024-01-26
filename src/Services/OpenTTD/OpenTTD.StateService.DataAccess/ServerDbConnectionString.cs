namespace OpenTTD.StateService.DataAccess;

public record ServerDbConnectionString
{
    public required string Value { get; set; }
}