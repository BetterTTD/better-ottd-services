using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Commands;

public sealed record ConnectServer(ServerId Id) : ICommand<ServerId>;