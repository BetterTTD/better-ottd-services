using Akka.Actor;
using Akka.DependencyInjection;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using OpenTTD.Actors.Server;
using Domain.Models;
using Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using OpenTTD.DataAccess;
using OpenTTD.Services;

namespace OpenTTD.Actors.Coordinator;

public sealed record ServerAdd(ServerCredentials Credentials);
public sealed record ServerConnect(ServerId ServerId);
public sealed record ServerDisconnect(ServerId ServerId);
public sealed record ServerRemove(ServerId ServerId);

public sealed record ServerAdded(ServerId Id);

public sealed class CoordinatorActor : ReceiveActor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();
    private readonly Dictionary<ServerId, (ServerCredentials Credentials, IActorRef Ref)> _servers = new();
    
    public CoordinatorActor(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
        
        ReceiveAsync<ServerAdd>(async msg =>
        {
            using var scope = scopeFactory.CreateScope();
            var serverService = scope.ServiceProvider.GetRequiredService<IServerConfigurationService>();
            var db = scope.ServiceProvider.GetRequiredService<AdminClientContext>();

            try
            {
                var serverIdRes = await serverService.AddServerAsync(msg.Credentials);

                if (!serverIdRes.IsSuccess)
                {
                    var (ip, port) = msg.Credentials.NetworkAddress;
                    
                    _logger.Error(serverIdRes.Exception, 
                        "Error while adding server {Ip}:{Port}", 
                        ip, port);
                    
                    Sender.Tell(Result.Failure<ServerAdded>(new ArgumentException("Server already exists")));
                    
                    return;
                }

                var serverId = serverIdRes.Value;
                
                AddServerActor(serverId, msg.Credentials);

                await db.SaveChangesAsync();
                
                _logger.Info(
                    "Server added: {NetworkAddress} with an id {Guid}",
                    msg.Credentials.NetworkAddress, serverId.Value);
            }
            catch (Exception exn)
            {
                var (ip, port) = msg.Credentials.NetworkAddress;
                    
                _logger.Error(exn, 
                    "Error while adding server {Ip}:{Port}", 
                    ip, port);
            }
            finally
            {
                scope.Dispose();
            }
        });

        Receive<ServerConnect>(msg =>
        {
            if (_servers.TryGetValue(msg.ServerId, out var data))
            {
                data.Ref.Tell(new Connect());
                _logger.Info(
                    "Server {ServerId} will be connected", 
                    msg.ServerId.Value);
            }
            else
            {
                _logger.Warning(
                    "Server {ServerId} was not found while connecting", 
                    msg.ServerId.Value);
            }
        });
        
        Receive<ServerDisconnect>(msg =>
        {
            if (_servers.TryGetValue(msg.ServerId, out var data))
            {
                data.Ref.Tell(new Disconnect());
                _logger.Info(
                    "Server {ServerId} will be disconnected", 
                    msg.ServerId.Value);
            }
            else
            {
                _logger.Warning(
                    "Server {ServerId} was not found while disconnecting", 
                    msg.ServerId.Value);
            }
        });
        
        ReceiveAsync<ServerRemove>(async msg =>
        {
            using var scope = scopeFactory.CreateScope();
            var serverService = scope.ServiceProvider.GetRequiredService<IServerConfigurationService>();
            var db = scope.ServiceProvider.GetRequiredService<AdminClientContext>();

            try
            {
                var serverIdRes = await serverService.DeleteServerAsync(msg.ServerId);

                if (!serverIdRes.IsSuccess)
                {
                    _logger.Error(serverIdRes.Exception, 
                        "Error while removing server {ServerId}", 
                        msg.ServerId.Value);
                    
                    return;
                }

                var serverId = serverIdRes.Value;
                
                if (_servers.TryGetValue(serverId, out var data))
                {
                    data.Ref.Tell(PoisonPill.Instance);
                    _servers.Remove(msg.ServerId);
                }
                else
                {
                    _logger.Warning(
                        "Server {ServerId} was not found while removing", 
                        msg.ServerId.Value);
                }

                await db.SaveChangesAsync();
                
                _logger.Info(
                    "Server {ServerId} has been removed", 
                    serverId.Value);
            }
            catch (Exception exn)
            {
                _logger.Error(exn, 
                    "Error while removing server {ServerId}", 
                    msg.ServerId.Value);
            }
            finally
            {
                scope.Dispose();
            }
        });
    }

    protected override async void PreStart()
    {
        using var scope = _scopeFactory.CreateScope();
        var serverService = scope.ServiceProvider.GetRequiredService<IServerConfigurationService>();

        try
        {
            var configurationsResult = await serverService.GetConfigurationsAsync();
            
            if (!configurationsResult.IsSuccess)
            {
                _logger.Error(configurationsResult.Exception, 
                    "Error while {PreStart} for {CoordinatorActor}", 
                    nameof(PreStart), nameof(CoordinatorActor));
                
                return;
            }

            configurationsResult.Value.ForEach(cfg => AddServerActor(
                cfg.Id,
                new ServerCredentials
                {
                    Name = cfg.Name,
                    NetworkAddress = new NetworkAddress(cfg.IpAddress, cfg.Port),
                    Password = cfg.Password,
                    Version = cfg.Version
                })
            );
        }
        catch (Exception exn)
        {
            _logger.Error(exn, 
                "Error while {PreStart} for {CoordinatorActor}", 
                nameof(PreStart), nameof(CoordinatorActor));
        }
        finally
        {
            base.PreStart();    
        }
    }

    private void AddServerActor(ServerId serverId, ServerCredentials credentials)
    {
        var serverProps = DependencyResolver.For(Context.System).Props<ServerActor>(serverId, credentials);
        var serverRef = Context.ActorOf(serverProps);

        _servers.Add(serverId, (credentials, serverRef));

        Sender.Tell(Result.Success(new ServerAdded(serverId)));
    }
}