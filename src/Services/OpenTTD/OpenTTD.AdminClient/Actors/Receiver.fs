module OpenTTD.AdminClient.Actors.Receiver


open System
open System.IO

open Akka.FSharp
open FSharpx.Collections

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

let init (stream : Stream) (mailbox : Actor<_>) =
    let rec loop () =
        actor {
            let! _  = mailbox.Receive ()
            let msg = waitForPacket stream |> packetToMsg
            mailbox.Context.Parent <! Message.PacketReceivedMsg msg
            return! loop () 
        }
        
    loop ()
