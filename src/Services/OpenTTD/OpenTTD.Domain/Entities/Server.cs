using CSharpFunctionalExtensions;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Entities;

public sealed class Server : Entity<ServerId>
{
    private readonly List<Company> _companies = [];

    public Server(ServerId id, ServerAddress address, ServerName name) : base(id)
    {
        Address = address;
        Name = name;
        
        var spectatorCompany = new Company(CompanyId.CreateSpectatorCompanyId());
        OpenNewCompany(spectatorCompany);
        
        var adminClient = new Client(ClientId.CreateForAdmin(), SpectatorCompany);
        SpectatorCompany.AttachClient(adminClient);
    }
    
    public IReadOnlyList<Company> Companies => _companies.AsReadOnly();
    
    public Company SpectatorCompany => _companies
        .Single(c => c.IsSpectator);
    public Client AdminClient => _companies
        .SelectMany(c => c.Clients)
        .Single(cl => cl.IsAdminClient);
    
    public IReadOnlyList<Client> Clients => _companies
        .SelectMany(c => c.Clients)
        .ToList()
        .AsReadOnly();

    public IReadOnlyList<Client> Spectators => _companies
        .Where(c => c.IsSpectator)
        .SelectMany(c => c.Clients)
        .ToList()
        .AsReadOnly();

    public ServerAddress Address { get; private init; }

    public ServerName Name { get; private init; }
    
    public void OpenNewCompany(Company company)
    {
        if (_companies.Any(c => c.Id == company.Id))
        {
            throw new InvalidOperationException($"Company {company.Id} already is in the list for server {Id}");
        }
        
        _companies.Add(company);
    }

    public void CloseCompany(CompanyId companyId)
    {
        var company = Companies.FirstOrDefault(c => c.Id == companyId);
        if (company is null)
        {
            throw new InvalidOperationException();
        }

        if (company.IsSpectator)
        {
            throw new InvalidOperationException();
        }

        var oldCompanyClients = new List<Client>(company.Clients);
        foreach (var client in oldCompanyClients)
        {
            client.ChangeCompany(SpectatorCompany);
        }

        _companies.Remove(company);
    }
}