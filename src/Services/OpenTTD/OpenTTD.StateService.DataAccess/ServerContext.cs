using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OpenTTD.StateService.DataAccess;

public partial class ServerContext(
    ILoggerFactory loggerFactory, 
    IOptions<OttdConnectionString> connectionString)
    : DbContext
{
    private readonly OttdConnectionString _connectionString = connectionString.Value;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseLoggerFactory(loggerFactory)
            .UseNpgsql(_connectionString.Value);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ServerContext).Assembly);
    }
}