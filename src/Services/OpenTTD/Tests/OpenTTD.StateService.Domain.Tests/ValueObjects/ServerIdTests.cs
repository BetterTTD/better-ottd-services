using OpenTTD.StateService.Domain.ValueObjects;

namespace OpenTTD.StateService.Domain.Tests.ValueObjects;

[TestFixture]
[TestOf(typeof(ServerId))]
public class ServerIdTests
{
    [Test]
    public void GenerateNew_ShouldCreateNewInstance()
    {
        // Assign

        var serverId = ServerId.GenerateNew();
        
        // Assert
        
        Assert.That(serverId, Is.Not.Null);
        Assert.That(serverId.Value, Is.Not.EqualTo(Guid.Empty));
    }
    
    [Test]
    public void WhenSameValue_ShouldBeEqual()
    {
        // Assign

        var guidValue = Guid.NewGuid();

        // Act

        var serverId1 = new ServerId(guidValue);
        var serverId2 = new ServerId(guidValue);

        // Assert
        
        Assert.That(serverId1, Is.EqualTo(serverId2));
    }
    
    [Test]
    public void WhenDifferentValue_ShouldBeNotEqual()
    {
        // Assign

        var guidValue1 = Guid.NewGuid();
        var guidValue2 = Guid.NewGuid();

        // Act

        var serverId1 = new ServerId(guidValue1);
        var serverId2 = new ServerId(guidValue2);

        // Assert
        
        Assert.That(serverId1, Is.Not.EqualTo(serverId2));
    }
}