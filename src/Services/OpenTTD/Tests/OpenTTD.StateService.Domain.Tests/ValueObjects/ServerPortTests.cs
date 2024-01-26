using OpenTTD.StateService.Domain.ValueObjects;

namespace OpenTTD.StateService.Domain.Tests.ValueObjects;

[TestFixture]
[TestOf(typeof(ServerPort))]
public class ServerPortTests
{
    [Test]
    public void Create_WhenDefault_ShouldBeZero()
    {
        // Assign

        var serverPort = ServerPort.Default();
        
        // Assert
        
        Assert.Multiple(() =>
        {
            Assert.That(serverPort.IsDefault, Is.True);
            Assert.That(serverPort.Value, Is.EqualTo(0));
        });
    }

    [Test]
    public void Create_WhenValueIsZeroOrGreater_ShouldCreate([Random(0, int.MaxValue, 1)] int value)
    {
        // Assign

        var serverPort = ServerPort.Create(value);

        // Assert

        Assert.That(serverPort, Is.Not.Null);
        Assert.That(serverPort.Value, Is.EqualTo(value));
    }
    
    [Test]
    public void Compare_WhenSameValues_ShouldBeEqual([Random(0, int.MaxValue, 1)] int portValue)
    {
        // Assign

        var port1 = ServerPort.Create(portValue);
        var port2 = ServerPort.Create(portValue);

        // Assert
        
        Assert.That(port1, Is.EqualTo(port2));
        Assert.That(port1.Value, Is.EqualTo(port2.Value));
    }

    [Test]
    public void Compare_WhenDifferentValues_ShouldNotBeEqual(
        [Random(0, 100, 1)] int portValue1,
        [Random(200, 300, 1)] int portValue2)
    {
        // Assign

        var port1 = ServerPort.Create(portValue1);
        var port2 = ServerPort.Create(portValue2);

        // Assert
        
        Assert.That(port1, Is.Not.EqualTo(port2));
        Assert.That(port1.Value, Is.Not.EqualTo(port2.Value));
    }
}