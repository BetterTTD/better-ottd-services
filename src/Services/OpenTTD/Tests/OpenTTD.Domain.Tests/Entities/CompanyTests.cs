using OpenTTD.Domain.Entities;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Tests.Entities;

[TestFixture]
[TestOf(typeof(Company))]
public class CompanyTests
{
    [Test]
    public void WhenCreatedAsSpectator_ShouldHaveSpectatorId()
    {
        // Assign

        var id = CompanyId.CreateSpectatorCompanyId();

        // Act

        var company = new Company(id);

        // Assert
        
        Assert.That(company.IsSpectator, Is.True);
    }

    [Test]
    public void AttachClient_WhenNewClient_ShouldAddToClients(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var spectatorCompany = new Company(CompanyId.CreateSpectatorCompanyId());
        var spectatorClient = new Client(ClientId.Create(clientId), spectatorCompany);
        
        var company = new Company(CompanyId.Create(companyId));

        // Act

        company.AttachClient(spectatorClient);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(company.Clients, Contains.Item(spectatorClient));
            Assert.That(spectatorCompany.Clients, Does.Not.Contain(spectatorClient));
        });
    }
    
    [Test]
    public void AttachClient_WhenSameExistingClient_ShouldFail(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var company = new Company(CompanyId.Create(companyId));
        var client = new Client(ClientId.Create(clientId), company);
        
        company.AttachClient(client);
        
        // Assert

        Assert.Throws<InvalidOperationException>(() => company.AttachClient(client));
    }

    [Test]
    public void DetachClient_WhenClientIsInClientsList_ShouldDetach(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign
        var company = new Company(CompanyId.Create(companyId));
        var client = new Client(ClientId.Create(clientId), company);
        company.AttachClient(client);
        
        // Act
        
        company.DetachClient(client.Id);
        
        // Assert
        
        Assert.That(company.Clients, Does.Not.Contain(client));
    }
    
    [Test]
    public void DetachClient_WhenCompanyIsSpectator_ShouldFail()
    {
        // Assign
        
        var spectatorCompany = new Company(CompanyId.CreateSpectatorCompanyId());
        var adminClient = new Client(ClientId.CreateForAdmin(), spectatorCompany);
        
        spectatorCompany.AttachClient(adminClient);
        
        // Assert

        Assert.Throws<InvalidOperationException>(() => spectatorCompany.DetachClient(adminClient.Id));
    }

    [Test]
    public void DetachClient_WhenClientIsNotInCompany_ShouldFail(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign
        
        var company = new Company(CompanyId.Create(companyId));
        var client = new Client(ClientId.Create(clientId), company);

        // Assert

        Assert.Throws<ArgumentException>(() => company.DetachClient(client.Id));
    }
}