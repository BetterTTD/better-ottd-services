using Microsoft.EntityFrameworkCore;
using OpenTTD.StateService.DataAccess.Entities;

namespace OpenTTD.StateService.DataAccess;

public partial class ServerContext
{
    public DbSet<ServerEntity> Servers { get; set; } = null!;
}