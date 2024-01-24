using System.Net;
using CSharpFunctionalExtensions;

namespace OpenTTD.Domain.ValueObjects;

public sealed class ServerAddress : ValueObject
{
    public IPAddress IpAddress { get; }

    public ServerPort Port { get; }

    public bool IsDefault => IpAddress.Equals(IPAddress.Parse("0.0.0.0")) && Port.IsDefault; 
    
    private ServerAddress(IPAddress ipAddress, ServerPort port)
    {
        IpAddress = ipAddress;
        Port = port;
    }

    public static ServerAddress Default() => new(IPAddress.Parse("0.0.0.0"), ServerPort.Default());

    public static ServerAddress Create(IPAddress ipAddress, ServerPort port) => new(ipAddress, port);
    
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return IpAddress.ToString();
        yield return Port.Value;
    }
}