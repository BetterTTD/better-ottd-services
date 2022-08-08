using MediatR;
using Microsoft.EntityFrameworkCore;
using OpenTTD.DataAccess;

namespace OpenTTD.Domain.Queries.GetServers;

public record GetServersHandler: IRequestHandler<GetServers, List<ServerViewModel>>
{
    private readonly OttdContext _dbContext;

    public GetServersHandler(OttdContext dbContext) => 
        _dbContext = dbContext;

    public async Task<List<ServerViewModel>> Handle(GetServers request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Servers
            .Select(server => new ServerViewModel())
            .ToListAsync(cancellationToken);
        
        return result;
    }
}