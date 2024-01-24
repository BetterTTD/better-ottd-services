using System.Net;
using CSharpFunctionalExtensions;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Entities;

public sealed class Client(ClientId id, Company company, IPAddress ipAddress) : Entity<ClientId>(id)
{
    public Company Company { get; private set; } = company;
    public IPAddress IpAddress { get; private set; } = ipAddress;
    
    // Todo: Update later
    public string Name { get; init; } = "Unknown";
    public byte Language { get; init; }
    public long JoinDate { get; init; }

    public bool IsAdminClient => Id.IsAdminClientId;

    public void ChangeCompany(Company toCompany)
    {
        if (IsAdminClient)
        {
            throw new InvalidOperationException("Admin client can be assigned only to Spectator company");
        }
        
        Company.DetachClient(Id);
        
        Company = toCompany;
        
        Company.AttachClient(this);
    }
}