using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTTD.StateService.DataAccess.Seeder;

namespace OpenTTD.StateService.DataAccess;

public static class MigrationManager
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        await using var ctx = scope.ServiceProvider.GetRequiredService<ServerContext>();

#if DEBUG
        var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();
        await ctx.Database.EnsureDeletedAsync();
#endif
        
        await ctx.Database.MigrateAsync();

#if DEBUG
        await seeder.SeedDataAsync();
#endif

        return host;
    }
}