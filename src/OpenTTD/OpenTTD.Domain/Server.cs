namespace OpenTTD.Domain;

public sealed record Server
{
    public string Name { get; init; } = "Unknown";
    public long Date { get; init; }
    public bool IsDedicated { get; set; }
    public Map Map { get; init; } = new();
    public ServerNetwork Network { get; init; } = new();
}