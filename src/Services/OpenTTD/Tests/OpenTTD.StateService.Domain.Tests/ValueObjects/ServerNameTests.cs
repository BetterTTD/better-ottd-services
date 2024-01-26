using OpenTTD.StateService.Domain.ValueObjects;

namespace OpenTTD.StateService.Domain.Tests.ValueObjects;

[TestFixture]
[TestOf(typeof(ServerName))]
public class ServerNameTests
{
    [Test]
    public void Created_WhenValueIsCorrect_ShouldCreate()
    {
        // Assign

        const string name = "Test Server Name";

        // Assign

        var serverName = ServerName.Create(name);
        
        // Assert
        
        Assert.That(serverName.Value, Is.EqualTo(name));
    }

    [Test]
    public void WhenValueIsSame_ShouldBeEqual()
    {
        // Assign

        const string name = "Test Server Name";

        // Assign

        var serverName1 = ServerName.Create(name);
        var serverName2 = ServerName.Create(name);
        
        // Assert
        
        Assert.That(serverName1, Is.EqualTo(serverName2));
    }
    
    [Test]
    public void Created_WhenValueIsNull_ShouldFail()
    {
        Assert.Throws<ArgumentException>(() => ServerName.Create(null!));
    }
    
    [Test]
    public void Created_WhenValueIsEmpty_ShouldFail()
    {
        Assert.Throws<ArgumentException>(() => ServerName.Create(string.Empty));
    }
    
    [Test]
    public void Created_WhenValueIsSpace_ShouldFail()
    {
        Assert.Throws<ArgumentException>(() => ServerName.Create(" "));
    }
}