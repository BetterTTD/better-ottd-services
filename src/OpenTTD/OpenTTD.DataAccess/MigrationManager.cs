using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenTTD.DataAccess;

public static class MigrationManager
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        await using var ctx = scope.ServiceProvider.GetRequiredService<AdminClientContext>();

        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.MigrateAsync();

        return host;
    }
}