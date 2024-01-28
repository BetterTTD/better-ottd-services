using System.Net;
using OpenTTD.StateService.DataAccess.Entities;

namespace OpenTTD.StateService.DataAccess.Seeder;

public class ServerDbSeeder(ServerContext db) : IDbSeeder
{
    public async Task SeedDataAsync()
    {
        var server = new ServerEntity
        {
            Id = Guid.NewGuid(),
            IpAddress = IPAddress.Parse("127.0.0.1"),
            Port = 3977,
            Password = "12345",
            Name = "BetterTTD",
            AdminName = "bttd_admin"
        };

        await db.Servers.AddAsync(server);

        await db.SaveChangesAsync();
    }
}