using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.DataAccess;

public sealed class AdminClientContext : DbContext
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly AdminClientConnectionString _connectionString;

    public DbSet<ServerConfiguration> ServerConfigurations { get; set; } = default!;

    public AdminClientContext(
        ILoggerFactory loggerFactory, 
        IOptions<AdminClientConnectionString> connectionStringOption)
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
        builder.ApplyConfigurationsFromAssembly(typeof(AdminClientContext).Assembly);
    }
}