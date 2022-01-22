module OpenTTD.AdminClient.Actors.Sender


open System.Net.Sockets
open Akka.FSharp

open Microsoft.Extensions.Logging
open OpenTTD.AdminClient.Networking.MessageTransformer
open OpenTTD.AdminClient.Networking.Packet
 
 
let init (loggerFactory : ILoggerFactory) (tcpClient : TcpClient) (mailbox : Actor<AdminMessage>) =
    
    let logger = loggerFactory.CreateLogger "Sender"
    logger.LogInformation "Initializing"
    
    let stream = tcpClient.GetStream()
    
    mailbox.Defer (fun _ ->
        logger.LogInformation "Stopping"
        stream.Dispose())

    let rec loop () =
        actor {      
            let! msg = mailbox.Receive ()

            logger.LogDebug $"Sending message: %A{msg}"            
            
            let { Buffer = buf; Size = size; } = msg |> msgToPacket |> prepareToSend
            stream.Write (buf, 0, int size)
            return! loop ()
        }
        
    loop ()
    