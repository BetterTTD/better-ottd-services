using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OpenTTD.StateService.DataAccess;

public class ServerContextFactory : IDesignTimeDbContextFactory<ServerContext>
{
    public ServerContext CreateDbContext(string[] args) => new(
        new LoggerFactory(),
        new OptionsWrapper<OttdConnectionString>(
            new OttdConnectionString
            {
                Value = "Host=localhost;Port=5432;Database=OpenTTDDB;Username=sa;Password=p@ssw0rd;"
            }
        )
    );
}