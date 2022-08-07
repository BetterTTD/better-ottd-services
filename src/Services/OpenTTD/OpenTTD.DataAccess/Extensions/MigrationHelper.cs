using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTTD.DataAccess.Seeder;

namespace OpenTTD.DataAccess.Extensions;

public static class HostExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync<TContext, TSeeder>(this IHost host, CancellationToken ct = default)
        where TContext : DbContext
        where TSeeder : IDbSeeder
    {
        using var scope = host.Services.CreateScope();
        await using var ctx = scope.ServiceProvider.GetRequiredService<TContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<TSeeder>();

        await ctx.Database.MigrateAsync(cancellationToken: ct);
        await seeder.SeedAsync(ct);

        return host;
    }
}