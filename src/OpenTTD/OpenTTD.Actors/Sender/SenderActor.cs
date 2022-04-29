﻿using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;
using OpenTTD.Networking.Messages;

namespace OpenTTD.Actors.Sender;

public sealed record SendMessage(IMessage Message);

public sealed class SenderActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public SenderActor(Stream stream, IPacketService packetService)
    {
        ReceiveAsync<SendMessage>(async msg =>
        {
            try
            {
                _logger.Debug($"{nameof(SenderActor)} sending a packet of type {msg.Message.PacketType}");
                
                var packet = packetService.CreatePacket(msg.Message);
                packet.PrepareToSend();

                await stream.WriteAsync(packet.Buffer.AsMemory(0, packet.Size));
            }
            catch (Exception e)
            {
                _logger.Error(e, $"{nameof(SenderActor)} received an error.");
            }
        });
    }
}