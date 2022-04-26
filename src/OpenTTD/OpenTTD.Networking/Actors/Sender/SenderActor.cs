using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;

namespace OpenTTD.Networking.Actors.Sender;

public sealed class SenderActor : ReceiveActor
{
    private readonly ILoggingAdapter _logger = Context.GetLogger<SerilogLoggingAdapter>();

    public SenderActor(Stream stream)
    {
        
    }
}