using System.Net;
using Domain.ValueObjects;

namespace OpenTTD.DataAccess.Models;

public class Server : DbModel<Guid>
{
    public ServerName Name { get; set; } = null!;
    public IPAddress IpAddress { get; set; } = null!;
    public ServerPort Port { get; set; } = null!;
    public ServerPassword Password { get; set; } = null!;
    public ServerVersion Version { get; set; } = null!;
}
