namespace OpenTTD.Domain;

public sealed record CompanyId(byte Value);

public sealed record Company
{
    public CompanyId Id { get; init; } = new(255);
    public string Name { get; init; } = "Unknown";
    public string ManagerName { get; init; } = "Unknown";
    public byte Color { get; init; }
    public bool HasPassword { get; init; }
    public long CreationDate { get; init; }
    public List<Client> Clients { get; init; } = new();

    public static Company Spectator => new()
    {
        Id = new CompanyId(255),
        Name = "Spectator",
        ManagerName = "",
        Color = 0,
        HasPassword = false,
        CreationDate = 0,
        Clients = new List<Client>()
    };
}