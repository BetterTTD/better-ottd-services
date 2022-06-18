using Akka.Util;
using Domain.Models;
using Domain.ValueObjects;
using OpenTTD.DataAccess.Models;

namespace OpenTTD.Services;

public interface IServerConfigurationService
{
    Task<Result<List<ServerConfiguration>>> GetConfigurationsAsync(CancellationToken ctx = default);
    Task<Result<Option<ServerConfiguration>>> GetConfigurationAsync(ServerId serverId, CancellationToken ctx = default);
    Task<Result<ServerId>> AddServerAsync(ServerCredentials credentials, CancellationToken ctx = default);
    Task<Result<ServerId>> DeleteServerAsync(ServerId serverId, CancellationToken ctx = default);
}