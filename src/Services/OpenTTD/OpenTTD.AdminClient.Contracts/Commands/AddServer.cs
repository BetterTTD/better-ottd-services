namespace OpenTTD.AdminClient.Contracts.Commands;

public record AddServer(
    Guid ServerId,
    string Name,
    string AdminName,
    string IpAddress,
    uint Port,
    string Password,
    string Version);