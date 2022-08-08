using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.Domain.Commands.ConnectServer;

public sealed record ConnectServerCommand(ServerId ServerId) : IRequest<ServerId>;