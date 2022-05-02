using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using OpenTTD.Networking.Common;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Actors.Receiver;

public sealed record ReceiveMsg;
public sealed record ReceivedMsg(IMessage Message);

public sealed class ReceiverActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();
    
    public ReceiverActor(Stream stream, IPacketService packetService)
    {
        ReceiveAsync<ReceiveMsg>(async _ =>
        {
            try
            {
                _logger.Debug($"{nameof(ReceiverActor)} receiving a package...");

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(45));
                var packet = await WaitForPacketAsync(stream, cts.Token);
                var message = packetService.ReadPacket(packet);
                
                Self.Tell(new ReceiveMsg(), Sender);
                _logger.Debug($"{nameof(ReceiverActor)} received {message.PacketType} message.");

                Context.Parent.Tell(new ReceivedMsg(message));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{nameof(ReceiverActor)} received an error.");
            }
        });
        
        Self.Tell(new ReceiveMsg(), Sender);
    }

    private static async Task<Packet> WaitForPacketAsync(Stream stream, CancellationToken token)
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

    private static async Task<byte[]> ReadAsync(Stream stream, int dataSize, CancellationToken token)
    {
        var result = new byte[dataSize];
        var currentSize = 0;

        do
        {
            await Task.Delay(TimeSpan.FromMilliseconds(1), token);
            
            currentSize += await stream.ReadAsync(result.AsMemory(currentSize, dataSize - currentSize), token);
            
        } while (currentSize < dataSize && !token.IsCancellationRequested);

        return result;
    }
}