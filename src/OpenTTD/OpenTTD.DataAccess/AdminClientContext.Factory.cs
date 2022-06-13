using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OpenTTD.DataAccess;

public sealed class AdminClientDesignTimeDbContextFactory : IDesignTimeDbContextFactory<AdminClientContext>
{
    public AdminClientContext CreateDbContext(string[] args) => new(
        new LoggerFactory(),
        new OptionsWrapper<AdminClientConnectionString>(
            new AdminClientConnectionString(
                "Server=localhost,1433;Initial Catalog=AdminClientDB;User ID=sa;Password=Strong_P@55w0rd"
            )
        )
    );
}