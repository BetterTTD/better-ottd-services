using System.Net;

namespace OpenTTD.DataAccess.Models;

public sealed record ServerName(string Value);
public sealed record ServerPassword(string Value);
public sealed record ServerPort(int Value);
public sealed record ServerVersion(string Value);

public class Server : DbModel<Guid>
{
    public ServerName Name { get; set; } = null!;
    public IPAddress IpAddress { get; set; } = null!;
    public ServerPort Port { get; set; } = null!;
    public ServerPassword Password { get; set; } = null!;
    public ServerVersion Version { get; set; } = null!;
}
