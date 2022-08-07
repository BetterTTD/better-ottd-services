using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.DataAccess;

public sealed class OttdContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly OttdDbConnectionString _connectionString;

    public DbSet<Server> Servers { get; set; } = null!;

    public OttdContext(ILoggerFactory loggerFactory, IOptions<OttdDbConnectionString> connectionStringOption)
    {
        _loggerFactory = loggerFactory;
        _connectionString = connectionStringOption.Value;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseLoggerFactory(_loggerFactory)
            .UseSqlServer(_connectionString.Value);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(OttdContext).Assembly);
    }
}