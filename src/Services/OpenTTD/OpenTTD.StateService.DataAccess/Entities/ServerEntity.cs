using System.Net;

namespace OpenTTD.StateService.DataAccess.Entities;

public class ServerEntity : Entity<Guid>
{
    public required string Name { get; set; }
    public required IPAddress IpAddress { get; set; }
    public required int Port { get; set; }
    public required string Password { get; set; }
}