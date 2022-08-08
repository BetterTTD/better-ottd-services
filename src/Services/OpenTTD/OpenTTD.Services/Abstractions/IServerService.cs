using CSharpFunctionalExtensions;
using Domain.Models;
using Domain.ValueObjects;

namespace OpenTTD.Services.Abstractions;

public interface IServerService
{
    Task<Result<ServerId, Exception>> AddServerAsync(ServerCredentials credentials, CancellationToken ct = default);
    Task<Result<ServerId, Exception>> RemoveServerAsync(ServerId serverId, CancellationToken ct = default);
}