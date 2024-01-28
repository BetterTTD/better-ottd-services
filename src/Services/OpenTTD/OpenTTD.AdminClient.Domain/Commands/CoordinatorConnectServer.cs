using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record CoordinatorConnectServer(ServerId Id) : ICommand<ServerId>;