using Akka.Util;
using Domain.Models;
using Domain.ValueObjects;
using MediatR;

namespace OpenTTD.AdminClient.Domain.Commands.AddServer;

public record AddServerCommand(ServerCredentials Credentials) : IRequest<Result<ServerId>>;