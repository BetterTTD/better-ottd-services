using System.Diagnostics.Contracts;
using System.Net;
using Common;
using Domain.Entities;
using Domain.Models;
using Domain.ValueObjects;
using OpenTTD.Networking.Messages;
using OpenTTD.Networking.Messages.Inbound.ServerClientError;
using OpenTTD.Networking.Messages.Inbound.ServerClientInfo;
using OpenTTD.Networking.Messages.Inbound.ServerClientQuit;
using OpenTTD.Networking.Messages.Inbound.ServerClientUpdate;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyInfo;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyRemove;
using OpenTTD.Networking.Messages.Inbound.ServerCompanyUpdate;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;

namespace Domain;

public interface IServerDispatcher
{
    [Pure]
    Server Create(ServerId serverId, ServerWelcomeMessage welcome, ServerProtocolMessage protocol);
    
    [Pure]
    Server Dispatch(IMessage message, Server server);
}

public sealed class ServerDispatcher : IServerDispatcher
{
    public Server Create(ServerId serverId, ServerWelcomeMessage welcome, ServerProtocolMessage protocol)
    {
        var server = new Server
        {
            Id = serverId,
            Name = new ServerName(welcome.ServerName),
            IsDedicated = welcome.IsDedicated,
            Date = welcome.CurrentDate,
            Map = new Map
            {
                Name = welcome.MapName,
                Landscape = welcome.Landscape,
                Width = welcome.MapWidth,
                Height = welcome.MapHeight
            },
            Network = new ServerNetwork
            {
                Version = protocol.NetworkVersion,
                Revision = welcome.NetworkRevision,
                UpdateFrequencies = protocol.AdminUpdateSettings,
            },
            Companies = new List<Company> { Company.Spectator }
        };
        
        return server;
    }

    public Server Dispatch(IMessage message, Server server) => message switch
    {
        ServerClientErrorMessage msg => F.Run(() =>
        {
            var company = server.Companies.FirstOrDefault(c => c.Clients.Any(cl => cl.Id.Value == msg.ClientId));
            if (company is null)
            {
                return server;
            }
            
            var updated = company with
            {
                Clients = company.Clients
                    .Where(cl => cl.Id.Value != msg.ClientId)
                    .ToList()
            };
            
            return server with
            {
                Companies = server.Companies
                    .Where(c => c.Id != company.Id)
                    .Append(updated)
                    .ToList()
            };
        }),
        
        ServerClientInfoMessage msg => F.Run(() =>
        { 
            _ = IPAddress.TryParse(msg.Hostname, out var address);

            var company = server.Companies.First(c => c.Id.Value == msg.CompanyId);
            var client = new Client
            {
                Id = new ClientId(msg.ClientId),
                Name = msg.ClientName,
                JoinDate = msg.JoinDate,
                Company = company,
                Language = msg.Language,
                Address = address ?? IPAddress.None
            };
            
            var updated = company with
            {
                Clients = company.Clients
                    .Where(cl => cl.Id.Value != msg.ClientId)
                    .Append(client)
                    .ToList()
            };
            
            return server with
            {
                Companies = server.Companies
                    .Where(c => c.Id != company.Id)
                    .Append(updated)
                    .ToList()
            };
        }),
        
        ServerClientUpdateMessage msg => F.Run(() =>
        {
            var company = server.Companies.First(c => c.Id.Value == msg.CompanyId);
            var client = company.Clients.First(cl => cl.Id.Value == msg.ClientId) with
            {
                Name = msg.ClientName,
                Company = company,
            };
            
            var updated = company with
            {
                Clients = company.Clients
                    .Where(cl => cl.Id.Value != msg.ClientId)
                    .Append(client)
                    .ToList()
            };
            
            return server with
            {
                Companies = server.Companies
                    .Where(c => c.Id != company.Id)
                    .Append(updated)
                    .ToList()
            };
        }),

        ServerClientQuitMessage msg => F.Run(() =>
        {
            var company = server.Companies.First(c => c.Clients.Any(cl => cl.Id.Value == msg.ClientId));
            var updated = company with
            {
                Clients = company.Clients
                    .Where(cl => cl.Id.Value != msg.ClientId)
                    .ToList()
            };
            
            return server with
            {
                Companies = server.Companies
                    .Where(c => c.Id != company.Id)
                    .Append(updated)
                    .ToList()
            };
        }),

        ServerCompanyInfoMessage msg => F.Run(() =>
        {
            var company = server.Companies.FirstOrDefault(c => c.Id.Value == msg.CompanyId, Company.Spectator) with
            {
                Id = new CompanyId(msg.CompanyId),
                Name = msg.CompanyName,
                ManagerName = msg.ManagerName,
                Color = msg.Color,
                HasPassword = msg.HasPassword
            };

            return server with
            {
                Companies = server.Companies
                    .Where(c => c.Id.Value != msg.CompanyId)
                    .Append(company)
                    .ToList()
            };
        }),
        
        ServerCompanyUpdateMessage msg => F.Run(() =>
        {
            var company = server.Companies.First(c => c.Id.Value == msg.CompanyId) with
            {
                Name = msg.CompanyName,
                ManagerName = msg.ManagerName,
                Color = msg.Color,
                HasPassword = msg.HasPassword
            };

            return server with
            {
                Companies = server.Companies
                    .Where(c => c.Id.Value != msg.CompanyId)
                    .Append(company)
                    .ToList()
            };
        }),
        
        ServerCompanyRemoveMessage msg => F.Run(() => server with
        {
            Companies = server.Companies
                .Where(c => c.Id.Value != msg.CompanyId)
                .ToList()
        }),

        _ => server
    };
}
