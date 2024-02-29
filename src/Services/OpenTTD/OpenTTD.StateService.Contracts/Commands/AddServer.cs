namespace OpenTTD.StateService.Contracts.Commands;

public record AddServer(
    string Name,
    string AdminName,
    string IpAddress,
    int Port,
    string Password);