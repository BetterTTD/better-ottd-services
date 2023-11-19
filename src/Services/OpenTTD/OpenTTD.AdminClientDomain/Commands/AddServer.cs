using OpenTTD.AdminClientDomain.Models;
using OpenTTD.AdminClientDomain.ValueObjects;

namespace OpenTTD.AdminClientDomain.Commands;

public sealed record AddServer(ServerCredentials Credentials) : ICommand<ServerId>;