using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using MediatR;
using OpenTTD.Actors.Server;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;

namespace OpenTTD.Actors.Coordinator;

public sealed class CoordinatorActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();
    
    public CoordinatorActor(IMediator mediator)
    {
        Dictionary<ServerId, (ServerCredentials Credentials, State ServerState, IActorRef Ref)> servers = new();
        
        Receive<ServerAdd>(msg =>
        {
            var maybeServerId = servers
                .Where(s =>
                    s.Value.Credentials.Name == msg.Credentials.Name ||
                    Equals(s.Value.Credentials.NetworkAddress.IpAddress, msg.Credentials.NetworkAddress.IpAddress) &&
                    s.Value.Credentials.NetworkAddress.Port == msg.Credentials.NetworkAddress.Port)
                .Select(x => x.Key)
                .FirstOrDefault();
            
            if (maybeServerId is not null)
            {
                _logger.Warning(
                    "[{Actor}] [{ServerId}] Server already added", 
                    nameof(CoordinatorActor), maybeServerId.Value);
                
                Sender.Tell(Result.Success(new ServerAdded(maybeServerId)));
            }
            else
            {
                var serverId = new ServerId(Guid.NewGuid());

                var serverProps = DependencyResolver
                    .For(Context.System)
                    .Props<ServerActor>(serverId, msg.Credentials);
                var serverRef = Context.ActorOf(serverProps);

                servers.Add(serverId, (msg.Credentials, State.IDLE, serverRef));
                
                _logger.Debug(
                    "[{Actor}] [{ServerId}] Server was added", 
                    nameof(CoordinatorActor), serverId.Value);
                
                Sender.Tell(Result.Success(new ServerAdded(serverId)));
            }
        });

        Receive<ServerConnect>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                if (data.ServerState is State.CONNECTED or State.CONNECTING)
                {
                    _logger.Warning(
                        "[{Actor}] [{ServerId}] Server is connected but connect called", 
                        nameof(CoordinatorActor), msg.ServerId.Value);
                    return;
                }
                
                _logger.Debug(
                    "[{Actor}] [{ServerId}] Server will be connected", 
                    nameof(CoordinatorActor), msg.ServerId.Value);
                data.Ref.Tell(new Connect());
            }
            else
            {
                _logger.Warning(
                    "[{Actor}] [{ServerId}] Server was not found while connecting", 
                    nameof(CoordinatorActor), msg.ServerId.Value);
            }
        });
        
        Receive<ServerDisconnect>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                if (data.ServerState is not State.CONNECTED)
                {
                    _logger.Warning(
                        "[{Actor}] [{ServerId}] Server is not connected but disconnect called", 
                        nameof(CoordinatorActor), msg.ServerId.Value);
                    return;
                }
                
                data.Ref.Tell(new Disconnect());
                _logger.Debug(
                    "[{Actor}] [{ServerId}] Server will be disconnected", 
                    nameof(CoordinatorActor), msg.ServerId.Value);
            }
            else
            {
                _logger.Warning(
                    "[{Actor}] [{ServerId}] Server was not found while disconnecting", 
                    nameof(CoordinatorActor), msg.ServerId.Value);
            }
        });
        
        Receive<ServerRemove>(msg =>
        {
            if (servers.TryGetValue(msg.ServerId, out var data))
            {
                data.Ref.Tell(PoisonPill.Instance);

                servers.Remove(msg.ServerId);
                
                _logger.Debug(
                    "[{Actor}] [{ServerId}] Server was added", 
                    nameof(CoordinatorActor), msg.ServerId.Value);
            }
            else
            {
                _logger.Warning(
                    "[{Actor}] [{ServerId}] Server was not found while remove", 
                    nameof(CoordinatorActor), msg.ServerId.Value);
            }
        });
    }
}