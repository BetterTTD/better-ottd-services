using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Domain.ValueObjects;
using OpenTTD.Networking.Messages;
using OpenTTD.Networking.Messages.Outbound.Ping;

namespace OpenTTD.Actors.Sender;

public sealed record SendMessage(IMessage Message);

public sealed class SenderActor : ReceiveActor, IWithTimers
{
    public ITimerScheduler Timers { get; set; } = default!;

    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public SenderActor(ServerId serverId, Stream stream, IPacketService packetService)
    {
        ReceiveAsync<SendMessage>(async msg =>
        {
            try
            {
                _logger.Debug(
                    "[{Guid}] Sending a packet of type {PacketType}", 
                    serverId.Value, msg.Message.PacketType);
                
                var packet = packetService.CreatePacket(msg.Message);
                packet.PrepareToSend();

                await stream.WriteAsync(packet.Buffer.AsMemory(0, packet.Size));
            }
            catch (Exception exn)
            {
                _logger.Error(exn, 
                    "[{Guid}] Received an error while sending a packet of type {PacketType}",
                    serverId.Value, msg.Message.PacketType);
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