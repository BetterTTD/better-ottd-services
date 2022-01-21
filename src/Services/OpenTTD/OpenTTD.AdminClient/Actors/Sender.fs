module OpenTTD.AdminClient.Actors.Sender


open System.IO

open Akka.FSharp

open OpenTTD.AdminClient.Networking.MessageTransformer
open OpenTTD.AdminClient.Networking.Packet
 
 
let init (stream : Stream) (mailbox : Actor<AdminMessage>) =
    
    printfn "[Sender:init]"
    
    let rec loop () =
        actor {      
            let! msg = mailbox.Receive ()
            
            printfn $"[Sender:send] msg: %A{msg}"
            
            let { Buffer = buf; Size = size; } = msg |> msgToPacket |> prepareToSend
            stream.Write (buf, 0, int size)
            return! loop ()
        }
        
    loop ()
    