using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record DisconnectServer(ServerId Id) : ICommand<ServerId>;