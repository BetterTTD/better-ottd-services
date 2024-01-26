using CSharpFunctionalExtensions;
using OpenTTD.StateService.Domain.Enums;
using OpenTTD.StateService.Domain.ValueObjects;

namespace OpenTTD.StateService.Domain.Entities;

public sealed class Company(CompanyId id) : Entity<CompanyId>(id)
{
    private readonly List<Client> _clients = [];

    public IReadOnlyList<Client> Clients => _clients.AsReadOnly();

    public bool IsSpectator => Id.IsSpectator;
    
    // TODO: Implement later
    public string Name { get; init; } = "Unknown";
    public string ManagerName { get; init; } = "Unknown";
    public CompanyColor CompanyColor { get; init; }
    public bool HasPassword { get; init; }
    public long CreationDate { get; init; }

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