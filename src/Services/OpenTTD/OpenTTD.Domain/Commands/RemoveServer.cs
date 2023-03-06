using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Commands;

public sealed record RemoveServer(ServerId Id) : ICommand<ServerId>;