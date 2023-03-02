using System.Net.Sockets;
using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Util;
using OpenTTD.Actors.Receiver;
using OpenTTD.Actors.Sender;
using Common;
using MediatR;
using OpenTTD.Domain.Models;
using OpenTTD.Domain.ValueObjects;
using OpenTTD.Networking.Messages.Inbound.ServerProtocol;
using OpenTTD.Networking.Messages.Inbound.ServerWelcome;
using OpenTTD.Networking.Messages.Outbound.Join;

namespace OpenTTD.Actors.Server;

public sealed partial class ServerActor
{
    private sealed record Idle(ServerId Id, ServerCredentials Credentials) : Model(Id, Credentials);

    private State<State, Model> IdleHandler(Event<Model> @event) => (@event.StateData, @event.FsmEvent) switch
    {
        (Idle(var serverId, var credentials), Connect) => F.Run(() =>
        {
            TryToConnect(serverId, credentials);
            return Stay();
        }),

        (Idle(var serverId, var credentials), Result<Unit> result) => F.Run(() =>
        {
            if (!result.IsSuccess)
            {
                return GoTo(State.ERROR).Using(new Error(serverId, credentials)
                {
                    Exception = result.Exception,
                    Message = $"Connection could not be established with address: {credentials.NetworkAddress}"
                });
            }
            
            _logger.Debug(
                "[ServerId:{ServerId}] Connection with address {Address} established successfully", 
                serverId.Value, credentials.NetworkAddress);

            var stream = _client.GetStream();

            var senderProps = DependencyResolver
                .For(Context.System)
                .Props<SenderActor>(serverId, stream);
            var receiverProps = DependencyResolver
                .For(Context.System)
                .Props<ReceiverActor>(serverId, stream);

            var network = new NetworkActors(
                Sender: Context.ActorOf(senderProps),
                Receiver: Context.ActorOf(receiverProps));

            network.Sender.Tell(new SendMessage(new JoinMessage
            {
                AdminName = credentials.Name,
                AdminVersion = credentials.Version,
                Password = credentials.Password
            }));

            var connectingState = new Connecting(
                serverId, credentials, network,
                Option<ServerProtocolMessage>.None,
                Option<ServerWelcomeMessage>.None);

            return GoTo(State.CONNECTING).Using(connectingState);
        }),

        var ((id, credentials), _) => F.Run(() => GoTo(State.ERROR).Using(new Error(id, credentials)
        {
            Exception = new InvalidOperationException(),
            Message = "Invalid state data"
        })),
        
        _ => throw new InvalidOperationException()
    };

    private void TryToConnect(ServerId serverId, ServerCredentials credentials) => Task.Run(async () =>
    {
        try
        {
            var address = credentials.NetworkAddress;

            _logger.Debug(
                "[ServerId:{ServerId}] Establishing connection with address {Address}",
                serverId.Value, address);

            await _client.ConnectAsync(address.IpAddress, address.Port);

            return Result.Success(Unit.Value);
        }
        catch (SocketException exn)
        {
            return Result.Failure<Unit>(exn);
        }
    }).PipeTo(Self, Sender);
}