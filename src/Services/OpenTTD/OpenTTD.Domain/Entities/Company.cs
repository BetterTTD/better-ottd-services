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

        _clients.Add(client);
    }

    public void DetachClient(ClientId clientId)
    {
        var client = _clients.SingleOrDefault(cl => cl.Id == clientId);

        if (client is null)
        {
            throw new ArgumentException($"Client with ID {clientId} was not found");
        }

        if (IsSpectator && client.IsAdminClient)
        {
            throw new InvalidOperationException("Admin client can't be moved out of Spectator company");
        }
        
        _clients.Remove(client);
    }
}