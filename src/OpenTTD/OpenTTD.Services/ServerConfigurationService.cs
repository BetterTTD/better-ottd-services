using Akka.Util;
using Akka.Util.Extensions;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using OpenTTD.DataAccess;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.Services;

public sealed class ServerConfigurationService : IServerConfigurationService
{
    private readonly AdminClientContext _dbContext;

    public ServerConfigurationService(AdminClientContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ServerConfiguration>>> GetConfigurationsAsync(CancellationToken ctx = default)
    {
        try
        {
            var list = await _dbContext.ServerConfigurations.ToListAsync(ctx); 
            return Result.Success(list);
        }
        catch (Exception exn)
        {
            return Result.Failure<List<ServerConfiguration>>(exn);
        }
    }

    public async Task<Result<Option<ServerConfiguration>>> GetConfigurationAsync(ServerId serverId, CancellationToken ctx = default)
    {
        try
        {
            var cfg = await _dbContext.ServerConfigurations.SingleOrDefaultAsync(sc => sc.Id == serverId, ctx);
            return Result.Success(cfg?.AsOption() ?? Option<ServerConfiguration>.None);
        }
        catch (Exception exn)
        {
            return Result.Failure<Option<ServerConfiguration>>(exn);
        }
    }

    public async Task<Result<ServerId>> AddServerAsync(ServerCredentials credentials, CancellationToken ctx = default)
    {
        try
        {
            var cfg = await _dbContext.ServerConfigurations
                .Where(sc => 
                    Equals(sc.IpAddress, credentials.NetworkAddress.IpAddress) &&
                    sc.Port == credentials.NetworkAddress.Port)
                .SingleOrDefaultAsync(ctx);

            if (cfg is not null)
            {
                return Result.Failure<ServerId>(new InvalidOperationException("Server already exists"));
            }

            var serverId = new ServerId(Guid.NewGuid());
            var dbo = new ServerConfiguration
            {
                Id = serverId,
                IpAddress = credentials.NetworkAddress.IpAddress,
                Port = credentials.NetworkAddress.Port,
                Name = credentials.Name,
                Version = credentials.Version,
                Password = credentials.Password
            };
        
            await _dbContext.AddAsync(dbo, ctx);
            return Result.Success(serverId);
        }
        catch (Exception exn)
        {
            return Result.Failure<ServerId>(exn);
        }
    }

    public async Task<Result<ServerId>> DeleteServerAsync(ServerId serverId, CancellationToken ctx = default)
    {
        try
        {
            var cfgResult = await GetConfigurationAsync(serverId, ctx);
            if (!cfgResult.IsSuccess)
            {
                return Result.Failure<ServerId>(cfgResult.Exception);
            }

            var cfg = cfgResult.Value;
            if (cfg == null)
            {
                return Result.Failure<ServerId>(
                    new ArgumentNullException(nameof(cfg), "Server not found. Nothing to delete"));
            }

            _dbContext.Remove(cfg);
                        
            return Result.Success(serverId);
        }
        catch (Exception exn)
        {
            return Result.Failure<ServerId>(exn);
        }
    }
}