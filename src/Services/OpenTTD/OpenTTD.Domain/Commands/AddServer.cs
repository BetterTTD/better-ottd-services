using Akka.Util;
using MediatR;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Domain.Commands;

public sealed record AddServer(ServerCredentials Credentials) : IRequest<Result<ServerId>>;