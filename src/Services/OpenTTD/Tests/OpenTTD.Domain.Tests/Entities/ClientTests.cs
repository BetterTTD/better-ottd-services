using System.Net;
using OpenTTD.Domain.Entities;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Tests.Entities;

[TestFixture]
[TestOf(typeof(Client))]
public class ClientTests
{
    [Test]
    public void WhenCreatedAsAdmin_ShouldHaveAdminId()
    {
        // Assign

        var spectator = new Company(CompanyId.CreateSpectatorCompanyId());
        var id = ClientId.CreateForAdmin();
        
        // Act

        var client = new Client(id, spectator, IPAddress.None);
        
        // Assert
        
        Assert.That(client.IsAdminClient, Is.True);
    }
    
    [Test]
    public void WhenCreated_ShouldHaveCreationId(
        [Random(2u, 255u, 1)] uint id)
    {
        // Assign

        var spectator = new Company(CompanyId.CreateSpectatorCompanyId());
        var clientId = ClientId.Create(id);
        
        // Act

        var client = new Client(clientId, spectator, IPAddress.None);
        
        // Assert
        
        Assert.That(client.Id.Value, Is.EqualTo(id));
    }

    [Test]
    public void ChangeCompany_WhenClientIsAdmin_ShouldFail(
        [Random(1u, 254u, 1)] uint companyId)
    {
        // Assign

        var spectator = new Company(CompanyId.CreateSpectatorCompanyId());
        var client = new Client(ClientId.CreateForAdmin(), spectator, IPAddress.None);
        var company = new Company(CompanyId.Create(companyId));
        
        spectator.AttachClient(client);
        
        // Assert

        Assert.Throws<InvalidOperationException>(() => client.ChangeCompany(company));
    }
    
    [Test]
    public void ChangeCompany_WhenClientIsNotAdmin_ShouldChange(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var spectator = new Company(CompanyId.CreateSpectatorCompanyId());
        var client = new Client(ClientId.Create(clientId), spectator, IPAddress.None);
        var company = new Company(CompanyId.Create(companyId));
        
        spectator.AttachClient(client);
        
        // Act

        client.ChangeCompany(company);
        
        // Assert
        
        Assert.Multiple(() =>
        {
            Assert.That(client.Company, Is.SameAs(company));
            Assert.That(spectator.Clients, Does.Not.Contain(client));
            Assert.That(company.Clients, Contains.Item(client));
        });
    }
}