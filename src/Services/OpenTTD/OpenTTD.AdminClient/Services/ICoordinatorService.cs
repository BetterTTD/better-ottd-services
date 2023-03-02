﻿using Akka.Util;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Services;

public interface ICoordinatorService
{
    Task<Result<ServerId>> AskToAddServerAsync(ServerCredentials credentials, CancellationToken cts);
    Task TellServerToConnectAsync(ServerId id, CancellationToken cts);
    Task TellServerToDisconnectAsync(ServerId id, CancellationToken cts);
    Task RemoveServerAsync(ServerId id, CancellationToken cts);
}