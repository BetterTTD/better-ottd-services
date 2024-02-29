using System.Net;
using DataAccess.Abstractions;

namespace OpenTTD.StateService.DataAccess.Entities;

public class ServerEntity : Entity<Guid>
{
    public required string Name { get; init; }
    public required IPAddress IpAddress { get; init; }
    public required int Port { get; init; }
    public required string Password { get; init; }
    public required string AdminName { get; init; }
}