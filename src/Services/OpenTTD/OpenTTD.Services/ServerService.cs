using CSharpFunctionalExtensions;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using OpenTTD.DataAccess;
using OpenTTD.DataAccess.Models;
using OpenTTD.Services.Abstractions;

namespace OpenTTD.Services;

public sealed class ServerService : IServerService
{
    private readonly OttdContext _context;

    public ServerService(OttdContext context)
    {
        _context = context;
    }

    public async Task<Result<ServerId, Exception>> AddServerAsync(ServerCredentials credentials, CancellationToken ct = default)
    {
        try
        {
            var server = await _context.Servers.SingleOrDefaultAsync(serv =>
                    serv.Name.Value == credentials.Name ||
                    serv.IpAddress == credentials.NetworkAddress.IpAddress &&
                    serv.Port.Value == credentials.NetworkAddress.Port,
                cancellationToken: ct);

            if (server is not null)
                return new ServerId(server.Id);

            var newServer = new Server
            {
                Name = new ServerName(credentials.Name),
                IpAddress = credentials.NetworkAddress.IpAddress,
                Port = new ServerPort(credentials.NetworkAddress.Port),
                Password = new ServerPassword(credentials.Password),
                Version = new ServerVersion(credentials.Version)
            };

            await _context.Servers.AddAsync(newServer, ct);

            await _context.SaveChangesAsync(ct);

            return new ServerId(newServer.Id);
        }
        catch (Exception exn)
        {
            return exn;
        }
    }

    public async Task<Result<ServerId, Exception>> RemoveServerAsync(ServerId serverId, CancellationToken ct = default)
    {
        try
        {
            var server = await _context.Servers.SingleOrDefaultAsync(serv => serv.Id == serverId.Value, ct);

            if (server is null)
                return new ArgumentException($"Server not found with ID: {serverId}", nameof(serverId));

            _context.Servers.Remove(server);

            await _context.SaveChangesAsync(ct);

            return serverId;
        }
        catch (Exception exn)
        {
            return exn;
        }
    }
}
