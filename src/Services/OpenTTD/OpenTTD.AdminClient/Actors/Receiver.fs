module OpenTTD.AdminClient.Actors.Receiver


open System
open System.IO

open System.Net.Sockets
open Akka.FSharp
open FSharpx.Collections

open Microsoft.Extensions.Logging
open OpenTTD.AdminClient.Networking.Packet
open OpenTTD.AdminClient.Networking.PacketTransformer
open OpenTTD.AdminClient.Models.ActorModels
 

let private read (stream : Stream) (size : int) =
    let buf = Array.zeroCreate<byte> size
    
    let rec tRead (tStream : Stream) (tSize : int) =
        if tSize < size then
            let res = tStream.Read (buf, tSize, size - tSize)
            tRead tStream (tSize + res)
        else tSize
        
    tRead stream 0 |> ignore
    buf

let private createPacket (sizeBuf : byte array) (content : byte array) =
    let buf = Array.zeroCreate<byte> (2 + content.Length)
    buf.[0] <- sizeBuf.[0]
    buf.[1] <- sizeBuf.[1]
    for i in 0 .. (content.Length - 1) do
        buf.[i + 2] <- content.[i]
    { createPacket with Buffer = buf }

let private waitForPacket (stream : Stream) =
    let sizeBuf = read stream 2
    let size = BitConverter.ToUInt16 (sizeBuf, 0)
    let content = read stream (int size - 2)
    createPacket sizeBuf content

let init (loggerFactory : ILoggerFactory) (tcpClient : TcpClient) (mailbox : Actor<_>) =
    
    let logger = loggerFactory.CreateLogger "Receiver"
    logger.LogInformation "Initializing"
    
    let stream = tcpClient.GetStream()
    
    mailbox.Defer (fun _ ->
        logger.LogInformation "Stopping"
        stream.Dispose())

    let rec loop () =
        actor {
            let! _  = mailbox.Receive ()
            try
                match waitForPacket stream |> packetToMsg with
                | Ok msg -> 
                    logger.LogDebug $"Received message: %A{msg}"
                    mailbox.Context.Parent <! Message.PacketReceivedMsg msg
                | Error err ->
                    logger.LogWarning $"Received unhandled packet with code: %d{err}"
                return! loop () 
            with
            | :? SocketException as ex when ex.ErrorCode = 10053 -> 
                return! loop () 
        }
        
    loop ()
