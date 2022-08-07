using System.Net;

namespace OpenTTD.DataAccess.Models;

public sealed record ServerName(string Value);
public sealed record ServerPassword(string Value);
public sealed record ServerPort(string Value);
public sealed record ServerVersion(string Value);

public class Server : DbModel<Guid>
{
    public ServerName Name { get; protected set; } = null!;
    public IPAddress IpAddress { get; protected set; } = null!;
    public ServerPort Port { get; protected set; } = null!;
    public ServerPassword Password { get; protected set; } = null!;
    public ServerVersion Version { get; protected set; } = null!;
}
