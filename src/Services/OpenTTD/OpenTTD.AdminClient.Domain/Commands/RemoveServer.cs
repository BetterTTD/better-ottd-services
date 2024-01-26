using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record RemoveServer(ServerId Id) : ICommand<ServerId>;