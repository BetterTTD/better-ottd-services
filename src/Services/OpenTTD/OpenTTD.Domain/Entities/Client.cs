using CSharpFunctionalExtensions;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Entities;

public sealed class Client(ClientId id, Company company) : Entity<ClientId>(id)
{
    public Company Company { get; private set; } = company;

    public bool IsAdminClient => Id.IsAdminClientId;

    public void AttachToCompany(Company newCompany)
    {
        if (Company.IsSpectator && IsAdminClient)
        {
            throw new InvalidOperationException("Admin client can be assigned only to Spectator company");
        }
        
        Company = newCompany;
    }
    
    public void ChangeCompany(Company toCompany)
    {
        Company.DetachClient(Id);
        
        toCompany.AttachClient(this);
    }
}