using System.Net;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.DataAccess.Seeder;

public sealed class OttdDbSeeder : IDbSeeder
{
    private readonly OttdContext _context;

    private readonly List<Server> _defaultServers = new()
    {
        new Server
        {
            Name = new ServerName("TG Local Test Server"),
            IpAddress = IPAddress.Parse("127.0.0.1"),
            Password = new ServerPassword("12345"),
            Port = new ServerPort(3977),
            Version = new ServerVersion("1.0-beta")
        }
    };

    public OttdDbSeeder(OttdContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var servers = await _context.Servers
            .Where(s => _defaultServers.Select(ds => ds.Name).Contains(s.Name))
            .ToListAsync(ct);

        var serversToAdd = _defaultServers
            .Where(ds => servers.All(s => s.Name != ds.Name))
            .ToList();

        await _context.Servers.AddRangeAsync(serversToAdd, ct);
        await _context.SaveChangesAsync(ct);
    }
}