using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenTTD.DataAccess.Extensions;

public static class HostExtensions
{
    public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host) where TContext : DbContext 
    {
        using var scope = host.Services.CreateScope();
        await using var ctx = scope.ServiceProvider.GetRequiredService<TContext>();

        await ctx.Database.MigrateAsync();

        return host;
    }
}