using OpenTTD.AdminClientDomain.ValueObjects;
using OpenTTD.Networking.Enums;

namespace OpenTTD.AdminClientDomain.Entities;

public sealed record Company : Entity<CompanyId>
{
    public string Name { get; init; } = "Unknown";
    public string ManagerName { get; init; } = "Unknown";
    public Color Color { get; init; }
    public bool HasPassword { get; init; }
    public long CreationDate { get; init; }
    public List<Client> Clients { get; init; } = [];

    public static Company Spectator => new()
    {
        Id = new CompanyId(255),
        Name = "Spectator",
        ManagerName = "",
        Color = Color.END,
        HasPassword = false,
        CreationDate = 0,
        Clients = []
    };
}