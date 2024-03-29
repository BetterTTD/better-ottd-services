﻿using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using OpenTTD.AdminClient.Actors.Base;
using OpenTTD.AdminClient.Domain.ValueObjects;

namespace OpenTTD.AdminClient.Actors;

public sealed record ServerAdd(ServerId Id, ServerNetwork Network) : IActorCommand;
public sealed record ServerConnect(ServerId ServerId) : IActorCommand;
public sealed record ServerDisconnect(ServerId ServerId) : IActorCommand;
public sealed record ServerRemove(ServerId ServerId) : IActorCommand;

public sealed record ServerAdded(ServerId Id) : IActorEvent;

public sealed class CoordinatorActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    private readonly Dictionary<ServerId, (ServerNetwork Credentials, State ServerState, IActorRef Ref)> _servers = new();
    
    public CoordinatorActor()
    {
        Receive<ServerAdd>(AddServer);
        Receive<ServerConnect>(ConnectServer);
        Receive<ServerDisconnect>(DisconnectServer);
        Receive<ServerRemove>(RemoveServer);
    }

    private void AddServer(ServerAdd msg)
    {
        var maybeServerId = _servers
            .Where(s =>
                s.Value.Credentials.AdminName == msg.Network.AdminName ||
                Equals(s.Value.Credentials.NetworkAddress.IpAddress, msg.Network.NetworkAddress.IpAddress) &&
                s.Value.Credentials.NetworkAddress.Port == msg.Network.NetworkAddress.Port)
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
            var serverId = msg.Id;

            var serverProps = DependencyResolver
                .For(Context.System)
                .Props<ServerActor>(serverId, msg.Network);
            var serverRef = Context.ActorOf(serverProps);

            _servers.Add(serverId, (msg.Network, State.IDLE, serverRef));
                
            _logger.Debug(
                "[{Actor}] [{ServerId}] Server was added", 
                nameof(CoordinatorActor), serverId.Value);
                
            Sender.Tell(Result.Success(new ServerAdded(serverId)));
        }
    }
    
    private void ConnectServer(ServerConnect msg)
    {
        if (_servers.TryGetValue(msg.ServerId, out var data))
        {
            if (data.ServerState is State.CONNECTED or State.CONNECTING)
            {
                _logger.Warning("[{Actor}] [{ServerId}] Server is connected but connect called", nameof(CoordinatorActor), msg.ServerId.Value);
                return;
            }

            _logger.Debug("[{Actor}] [{ServerId}] Server will be connected", nameof(CoordinatorActor), msg.ServerId.Value);
            data.Ref.Tell(new Connect());
        }
        else
        {
            _logger.Warning("[{Actor}] [{ServerId}] Server was not found while connecting", nameof(CoordinatorActor), msg.ServerId.Value);
        }
    }

    private void DisconnectServer(ServerDisconnect msg)
    {
        if (_servers.TryGetValue(msg.ServerId, out var data))
        {
            if (data.ServerState is not State.CONNECTED)
            {
                _logger.Warning("[{Actor}] [{ServerId}] Server is not connected but disconnect called", nameof(CoordinatorActor), msg.ServerId.Value);
                return;
            }

            data.Ref.Tell(new Disconnect());
            _logger.Debug("[{Actor}] [{ServerId}] Server will be disconnected", nameof(CoordinatorActor), msg.ServerId.Value);
        }
        else
        {
            _logger.Warning("[{Actor}] [{ServerId}] Server was not found while disconnecting", nameof(CoordinatorActor), msg.ServerId.Value);
        }
    }

    private void RemoveServer(ServerRemove msg)
    {
        if (_servers.TryGetValue(msg.ServerId, out var data))
        {
            data.Ref.Tell(PoisonPill.Instance);

            _servers.Remove(msg.ServerId);

            _logger.Debug("[{Actor}] [{ServerId}] Server was added", nameof(CoordinatorActor), msg.ServerId.Value);
        }
        else
        {
            _logger.Warning("[{Actor}] [{ServerId}] Server was not found while remove", nameof(CoordinatorActor), msg.ServerId.Value);
        }
    }
}