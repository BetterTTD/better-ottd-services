﻿using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using MediatR;
using OpenTTD.AdminClient.Domain.Events;
using OpenTTD.AdminClient.Domain.ValueObjects;
using OpenTTD.AdminClient.Networking.Messages;
using OpenTTD.AdminClient.Networking.Messages.Outbound.Ping;

namespace OpenTTD.AdminClient.Actors.Sender;

public sealed class SenderActor : ReceiveActor, IWithTimers
{
    public ITimerScheduler Timers { get; set; } = default!;

    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public SenderActor(ServerId serverId, Stream stream, IPacketService packetService, IMediator mediator)
    {
        ReceiveAsync<SendMessage>(async msg =>
        {
            try
            {
                _logger.Debug(
                    "[{Actor}] [ServerId:{ServerId}] Sending a packet of type {PacketType}", 
                    nameof(SenderActor), serverId.Value, msg.Message.PacketType);
                
                var packet = packetService.CreatePacket(msg.Message);
                packet.PrepareToSend();

                await stream.WriteAsync(packet.Buffer.AsMemory(0, packet.Size));
                
                _logger.Debug(
                    "[{Actor}] [ServerId:{ServerId}] Packet of type {PacketType} was sent successfully", 
                    nameof(SenderActor), serverId.Value, msg.Message.PacketType);

                await mediator.Publish(new NetworkMessageSent(serverId, msg.Message));
            }
            catch (Exception exn)
            {
                _logger.Error(exn, 
                    "[{Actor}] [ServerId:{ServerId}] Received an error while sending a packet of type {PacketType}",
                    nameof(SenderActor), serverId.Value, msg.Message.PacketType);
            }
        });
    }

    protected override void PreStart()
    {
        base.PreStart();
        Timers.StartPeriodicTimer(
            nameof(SenderActor), 
            new SendMessage(new PingMessage()), 
            TimeSpan.FromSeconds(10));
    }

    protected override void PostStop()
    {
        base.PostStop();
        Timers.CancelAll();
    }
}