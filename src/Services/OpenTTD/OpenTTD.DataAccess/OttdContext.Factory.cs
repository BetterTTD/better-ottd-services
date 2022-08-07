using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OpenTTD.DataAccess;

// ReSharper disable once UnusedType.Global
public class OttdContextFactory : IDesignTimeDbContextFactory<OttdContext>
{
    public OttdContext CreateDbContext(string[] args) => new(
        new LoggerFactory(),
        new OptionsWrapper<OttdDbConnectionString>(
            new OttdDbConnectionString
            {
                Value = "Server=localhost,1433;Initial Catalog=OpenTTD_DB;User ID=sa;Password=Your_password123"
            }
        )
    );
}