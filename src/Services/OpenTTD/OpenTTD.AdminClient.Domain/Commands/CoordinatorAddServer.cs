using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Domain.Commands;

public sealed record CoordinatorAddServer(ServerId Id, ServerNetwork Network) : ICommand<ServerId>;