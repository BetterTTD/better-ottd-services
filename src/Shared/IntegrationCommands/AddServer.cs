namespace IntegrationCommands;

public record AddServer(
    string Name,
    string AdminName,
    string IpAddress,
    uint Port,
    string Password);