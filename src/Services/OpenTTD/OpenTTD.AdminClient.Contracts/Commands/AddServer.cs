namespace OpenTTD.AdminClient.Contracts.Commands;

public record AddServer(
    Guid ServerId,
    string Name,
    string AdminName,
    string IpAddress,
    int Port,
    string Password,
    string Version);