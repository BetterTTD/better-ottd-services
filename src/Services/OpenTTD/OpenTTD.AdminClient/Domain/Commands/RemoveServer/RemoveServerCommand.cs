using Akka.Util;
using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.AdminClient.Domain.Commands.RemoveServer;

public record RemoveServerCommand(ServerId ServerId) : IRequest<Result<ServerId>>;