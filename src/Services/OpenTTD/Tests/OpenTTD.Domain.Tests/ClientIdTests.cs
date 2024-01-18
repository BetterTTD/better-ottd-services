using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Tests;

public class ClientIdTests
{
    [Test]
    public void Create_WhenAdminClient_ShouldReturnId1()
    {
        // Assign
        
        var clientId = ClientId.CreateForAdmin();

        // Assert
        
        Assert.That(clientId.Value, Is.EqualTo(1));
    }
    
    [Test]
    public void Create_WhenHasCorrectRangeValues_ShouldReturnId([Range(2u, 255u)] uint value)
    {
        // Assign
        
        var clientId = ClientId.Create(value);

        // Assert
        
        Assert.That(clientId.Value, Is.EqualTo(value));
    }
    
    [Test]
    public void Create_WhenHasOutOfRangeValues_ShouldFail([Values(1u, 256u)] uint value)
    {
        Assert.Throws<ArgumentException>(() => ClientId.Create(value));
    }
}