using System.Net;
using OpenTTD.Domain.Entities;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Tests.Entities;

[TestFixture]
[TestOf(typeof(Server))]
public class ServerTests
{
    [Test]
    public void WhenCreated_ShouldHaveOnlySpectatorCompany()
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        
        // Act
        
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(server.Companies, Has.One.Items);
            Assert.That(server.SpectatorCompany, Is.Not.Null);
        });
    }

    [Test]
    public void WhenCreated_ShouldHaveOnlyAdminClient()
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        
        // Act
        
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        
        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(server.Clients, Has.One.Items);
            Assert.That(server.Spectators, Contains.Item(server.AdminClient));
            Assert.That(server.AdminClient, Is.Not.Null);
        });
    }

    [Test]
    public void OpenNewCompany_WhenCompanyIsNotSpectator_ShouldAddCompany(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        var company = new Company(CompanyId.Create(companyId));
        var client = new Client(ClientId.Create(clientId), company, IPAddress.None);
        
        // Act
        
        company.AttachClient(client);
        server.OpenNewCompany(company);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(server.Companies, Has.Exactly(2).Items);
            Assert.That(server.Clients, Has.Exactly(2).Items);
            Assert.That(server.Spectators, Has.Exactly(1).Items);
            Assert.That(server.Companies, Contains.Item(company));
            Assert.That(server.Clients, Contains.Item(client));
        });
    }

    [Test]
    public void OpenNewCompany_WhenCompanyIsSpectator_ShouldFail()
    {
        // Assign
        
        var serverId = new ServerId(Guid.Empty);
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        var company = new Company(CompanyId.CreateSpectatorCompanyId());
        
        // Assert
        
        Assert.Throws<InvalidOperationException>(() => server.OpenNewCompany(company));
    }

    [Test]
    public void OpenCompany_WhenCompanyAlreadyExists_ShouldFail(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        
        var company = new Company(CompanyId.Create(companyId));
        
        // Act
        
        server.OpenNewCompany(company);

        // Assert
        
        Assert.Throws<InvalidOperationException>(() => server.OpenNewCompany(company));
    }

    [Test]
    public void CloseCompany_WhenCompanyIsNotSpectator_ShouldRemove(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        var company = new Company(CompanyId.Create(companyId));
        var client = new Client(ClientId.Create(clientId), company, IPAddress.None);
        
        company.AttachClient(client);
        server.OpenNewCompany(company);
        
        // Act
        
        server.CloseCompany(company.Id);
        
        // Assert
        
        Assert.Multiple(() =>
        {
            Assert.That(server.Companies, Has.Exactly(1).Items);
            Assert.That(server.Companies, Does.Not.Contain(company));
            Assert.That(server.Clients, Has.Exactly(2).Items);
            Assert.That(server.Spectators, Has.Exactly(2).Items);
            Assert.That(server.Spectators, Does.Contain(client));
        });
    }
    
    [Test]
    public void CloseCompany_WhenCompanyIsSpectator_ShouldFail(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        
        // Assert

        Assert.Throws<InvalidOperationException>(() => server.CloseCompany(server.SpectatorCompany.Id));
    }
    
    [Test]
    public void CloseCompany_WhenCompanyIsNotExists_ShouldFail(
        [Random(1u, 254u, 1)] uint companyId, 
        [Random(2u, 255u, 1)] uint clientId)
    {
        // Assign

        var serverId = new ServerId(Guid.Empty);
        var server = new Server(serverId, ServerAddress.Default(), ServerName.Default());
        var company = new Company(CompanyId.Create(companyId));
        
        // Assert

        Assert.Throws<InvalidOperationException>(() => server.CloseCompany(company.Id));
    }
}