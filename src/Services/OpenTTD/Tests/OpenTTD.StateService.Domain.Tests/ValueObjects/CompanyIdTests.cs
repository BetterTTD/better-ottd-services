using OpenTTD.StateService.Domain.ValueObjects;

namespace OpenTTD.StateService.Domain.Tests.ValueObjects;

[TestFixture]
[TestOf(typeof(CompanyId))]
public class CompanyIdTests
{
    [Test]
    public void Create_WhenSpectatorCompany_ShouldReturnId255()
    {
        // Assign
        
        var clientId = CompanyId.CreateSpectatorCompanyId();
        
        // Assert
        
        Assert.That(clientId.Value, Is.EqualTo(255));
    }
    
    [Test]
    public void Create_WhenHasCorrectRangeValues_ShouldReturnId([Range(1u, 254u)] uint value)
    {
        // Assign
        
        var clientId = CompanyId.Create(value);

        // Assert
        
        Assert.That(clientId.Value, Is.EqualTo(value));
    }
    
    [Test]
    public void Create_WhenHasOutOfRangeValues_ShouldFail([Values(0u, 255u)] uint value)
    {
        Assert.Throws<ArgumentException>(() => CompanyId.Create(value));
    }
}