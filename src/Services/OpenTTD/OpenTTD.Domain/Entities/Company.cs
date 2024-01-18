using CSharpFunctionalExtensions;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Entities;

public sealed class Company(CompanyId id) : Entity<CompanyId>(id)
{
    private readonly List<Client> _clients = [];

    public IReadOnlyList<Client> Clients => _clients.AsReadOnly();

    public bool IsSpectator => Id.IsSpectator;

    public void AttachClient(Client client)
    {
        if (_clients.Any(cl => cl == client))
        {
            throw new InvalidOperationException($"Client with ID {client.Id} already exists");
        }

        if (client.Company != this)
        {
            client.AttachToCompany(this);    
        }
        
        _clients.Add(client);
    }

    public void DetachClient(ClientId clientId)
    {
        var client = _clients.SingleOrDefault(cl => cl.Id == clientId);

        if (client is null)
        {
            throw new InvalidOperationException($"Client with ID {clientId} was not found");
        }

        _clients.Remove(client);
    }
}