using System.Net;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Tests.ValueObjects;

[TestFixture]
[TestOf(typeof(ServerAddress))]
public class ServerAddressTests
{
    [Test]
    public void WhenDefault_ShouldHaveDefaultValues()
    {
        // Assign

        var serverAddress = ServerAddress.Default();
        
        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(serverAddress.IsDefault, Is.True);
            Assert.That(serverAddress.IpAddress, Is.EqualTo(IPAddress.Parse("0.0.0.0")));
            Assert.That(serverAddress.Port, Is.EqualTo(ServerPort.Default()));
        });
    }

    [Test]
    public void WhenHaveSameValues_ShouldBeEqual()
    {
        // Assign

        var ipAddress = IPAddress.Parse("127.0.0.1");
        var port = ServerPort.Create(3977);

        // Act

        var serverAddress1 = ServerAddress.Create(ipAddress, port);
        var serverAddress2 = ServerAddress.Create(ipAddress, port);

        // Assert
        
        Assert.That(serverAddress1, Is.EqualTo(serverAddress2));
    }

    [Test]
    public void WhenHaveDifferentIpAddress_ShouldNotBeEqual()
    {
        // Assign

        var ipAddress1 = IPAddress.Parse("127.0.0.1");
        var ipAddress2 = IPAddress.Parse("127.0.0.2");
        var port = ServerPort.Create(3977);

        // Act

        var serverAddress1 = ServerAddress.Create(ipAddress1, port);
        var serverAddress2 = ServerAddress.Create(ipAddress2, port);

        // Assert
        
        Assert.That(serverAddress1, Is.Not.EqualTo(serverAddress2));
    }

    [Test]
    public void WhenHaveDifferentPort_ShouldNotBeEqual()
    {
        // Assign

        var ipAddress = IPAddress.Parse("127.0.0.1");
        var port1 = ServerPort.Create(3977);
        var port2 = ServerPort.Create(3978);

        // Act

        var serverAddress1 = ServerAddress.Create(ipAddress, port1);
        var serverAddress2 = ServerAddress.Create(ipAddress, port2);

        // Assert
        
        Assert.That(serverAddress1, Is.Not.EqualTo(serverAddress2));
    }

    [Test]
    public void WhenHaveDifferentValue_ShouldNotBeEqual()
    {
        // Assign

        var ipAddress1 = IPAddress.Parse("127.0.0.1");
        var ipAddress2 = IPAddress.Parse("127.0.0.2");
        var port1 = ServerPort.Create(3977);
        var port2 = ServerPort.Create(3978);

        // Act

        var serverAddress1 = ServerAddress.Create(ipAddress1, port1);
        var serverAddress2 = ServerAddress.Create(ipAddress2, port2);

        // Assert
        
        Assert.That(serverAddress1, Is.Not.EqualTo(serverAddress2));
    }
}