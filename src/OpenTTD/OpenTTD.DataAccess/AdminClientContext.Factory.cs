using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OpenTTD.DataAccess;

public sealed class AdminClientDesignTimeDbContextFactory : IDbContextFactory<AdminClientContext>
{
    public AdminClientContext CreateDbContext() => new(
        new LoggerFactory(),
        new OptionsWrapper<AdminClientConnectionString>(
            new AdminClientConnectionString(
                "Server=localhost,1433;Initial Catalog=AdminClientDB;User ID=sa;Password=Strong_P@55w0rd"
            )
        )
    );
}