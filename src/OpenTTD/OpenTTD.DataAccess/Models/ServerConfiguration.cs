using System.Net;
using OpenTTD.DataAccess.Models.Base;

namespace OpenTTD.DataAccess.Models;

public class ServerConfiguration : Modifiable<int>
{
    public IPAddress IpAddress { get; init; } = IPAddress.None;
    public int Port { get; init; } = default;
    public string Name { get; init; } = string.Empty;
    public string Version { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}