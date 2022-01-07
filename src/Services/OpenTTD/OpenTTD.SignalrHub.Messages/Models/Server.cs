namespace OpenTTD.SignalrHub.Messages.Models;

public record ServerMap
{
    public int Seed { get; init; }
    public object Landscape { get; init; }
    public object StartDate { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
}

public record ServerInfo
{
    public string Ip { get; init; }
    public int Port { get; init; }
    public string? Name { get; init; }
    public string? Version { get; init; }
    public ServerMap? Map { get; init; }
}

public record Client
{
    public int Id { get; init; }
    public string Ip { get; init; }
    public string Name { get; init; }
    public object Language { get; init; }
    public object JoinDate { get; init; }
    public int CompanyId { get; init; }
}

public record Server
{
    public Guid Id { get; init; }
    public ServerInfo Info { get; init; }
    public Client[] Clients { get; init; } = Array.Empty<Client>();
}
