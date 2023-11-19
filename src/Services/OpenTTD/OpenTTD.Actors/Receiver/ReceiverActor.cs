using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Util;
using MediatR;
using OpenTTD.AdminClientDomain.Events;
using OpenTTD.AdminClientDomain.ValueObjects;
using OpenTTD.Networking.Common;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Actors.Receiver;

public sealed class ReceiverActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();
    
    public ReceiverActor(ServerId serverId, Stream stream, IPacketService packetService, IMediator mediator)
    {
        ReceiveAsync<ReceiveMsg>(async _ =>
        {
            try
            {
                _logger.Debug(
                    "[{Actor}] [ServerId:{ServerId}] Receiving a package...", 
                    nameof(ReceiverActor), serverId.Value);

                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
                var packet = await WaitForPacketAsync(stream, cts.Token);
                var message = packetService.ReadPacket(packet);

                _logger.Debug(
                    "[{Actor}] [ServerId:{ServerId}] Received a package of type {PacketType}", 
                    nameof(ReceiverActor), serverId.Value, message.PacketType);

                Self.Tell(new ReceiveMsg(), Sender);

                await mediator.Publish(new NetworkMessageReceived(serverId, message));

                Context.Parent.Tell(new ReceivedMsg(Result.Success(message)));
            }
            catch (OperationCanceledException exn)
            {
                _logger.Error(exn, 
                    "[{Actor}] [ServerId:{ServerId}] Timeout handled", 
                    nameof(ReceiverActor), serverId.Value);
                Context.Parent.Tell(new ReceivedMsg(Result.Failure<IMessage>(exn)));
            }
            catch (Exception exn)
            {
                _logger.Error(exn, 
                    "[{Actor}] [ServerId:{ServerId}] Received an error while receiving a packet", 
                    nameof(ReceiverActor), serverId.Value);
                Context.Parent.Tell(new ReceivedMsg(Result.Failure<IMessage>(exn)));
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