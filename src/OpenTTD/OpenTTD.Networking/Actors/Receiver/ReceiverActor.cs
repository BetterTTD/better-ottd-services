using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using OpenTTD.Networking.AdminPort;
using System;

namespace OpenTTD.Networking.Actors.Receiver;

public record ReceiveMsg;

public sealed class ReceiverActor : ReceiveActor, IWithTimers
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();
    
    public ITimerScheduler Timers { get; set; }
    
    public ReceiverActor(Stream stream)
    {
        ReceiveAsync<ReceiveMsg>(async _ =>
        {
            try
            {
                var packet = await WaitForPacketAsync(stream, CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error while receiving");
                throw;
            }
        });
    }

    protected override void PreStart()
    {
        base.PreStart();
        
        Timers.StartPeriodicTimer(nameof(ReceiveActor), new ReceiveMsg(), TimeSpan.FromSeconds(1));
    }

    private async Task<Packet> WaitForPacketAsync(Stream stream, CancellationToken token)
    {
        var sizeBuffer = await ReadAsync(stream, 2, token);

        var size = BitConverter.ToUInt16(sizeBuffer, 0);
        var content = await ReadAsync(stream, size - 2, token);
        
        var packet = CreatePacket(sizeBuffer, content);
        return packet;
    }
    
    private static Packet CreatePacket(IReadOnlyList<byte> sizeBuffer, IReadOnlyList<byte> content)
    {
        var packetData = new byte[2 + content.Count];
        packetData[0] = sizeBuffer[0];
        packetData[1] = sizeBuffer[1];
        
        for (var i = 0; i < content.Count; ++i)
            packetData[2 + i] = content[i];

        var packet = new Packet(packetData);
        return packet;
    }

    private async Task<byte[]> ReadAsync(Stream stream, int dataSize, CancellationToken token)
    {
        var result = new byte[dataSize];
        var currentSize = 0;

        do
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1), token);
            
            currentSize += await stream.ReadAsync(result.AsMemory(currentSize, dataSize - currentSize), token);
            
            _logger.Debug($"Receiver trying to receive packet({token.IsCancellationRequested})");
            
        } while (currentSize < dataSize && !token.IsCancellationRequested);

        return result;
    }
}