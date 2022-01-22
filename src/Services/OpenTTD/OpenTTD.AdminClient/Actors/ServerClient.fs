module OpenTTD.AdminClient.Actors.ServerClient


open System
open System.Net.Sockets

open Akka.Event
open Akka.Actor
open Akka.FSharp

open Microsoft.Extensions.Logging
open OpenTTD.AdminClient.Models
open OpenTTD.AdminClient.Models.ActorModels
open OpenTTD.AdminClient.Models.Configurations
open OpenTTD.AdminClient.Networking.PacketTransformer
open OpenTTD.AdminClient.Networking.MessageTransformer


type private Actors =
    { Sender    : IActorRef
      Receiver  : IActorRef
      Scheduler : IActorRef }

let init (logger : ILogger) (cfg : ServerConfiguration) (mailbox : Actor<Message>) =

    logger.LogInformation $"[ServerClient:init] cfg: %A{cfg}"
    
    let stream =
        let tcpClient = new TcpClient ()
        tcpClient.Connect (cfg.Host, cfg.Port)
        tcpClient.GetStream ()
        
    let actors =
        {  Sender    = Sender.init    logger stream |> spawn mailbox "sender"
           Receiver  = Receiver.init  logger stream |> spawn mailbox "receiver"
           Scheduler = Scheduler.init logger        |> spawn mailbox "scheduler" }
    
    mailbox.Defer (fun _ ->
        logger.LogInformation $"[ServerClient:stopping] Taking pill instances for: %A{cfg}"
        actors.Scheduler <! PoisonPill.Instance
        actors.Sender    <! PoisonPill.Instance
        actors.Receiver  <! PoisonPill.Instance
        stream.Dispose ())

    
    let rec errored actors state =
        actor {
                    
            logger.LogInformation "[ServerClient:errored]"
            
            actors.Scheduler <! Scheduler.PauseJob
            return! errored actors state
        }

    and connected actors state =
        actor {
            
            logger.LogInformation "[ServerClient:connected]"

            match! mailbox.Receive () with
            | PacketReceivedMsg msg ->
                let state = State.dispatch state msg
                return! connected actors state
            | _ -> return UnhandledMessage
        }
        
    and connecting actors state =
        actor {
            
            logger.LogInformation "[ServerClient:connecting]"

            match! mailbox.Receive () with
            | PacketReceivedMsg msg ->
                let state = State.dispatch state msg
                match msg with
                | ServerProtocolMsg _ ->
                    defaultPolls @ defaultUpdateFrequencies |> List.iter (fun msg -> actors.Sender <! msg)
                    return! connecting actors state
                | ServerWelcomeMsg _ ->
                    return! connected actors state
                | _ ->
                    return UnhandledMessage
            | _ -> return UnhandledMessage
        }
        
    and idle actors state =
        actor {
            
            logger.LogInformation "[ServerClient:idle]"

            match! mailbox.Receive () with
            | AuthorizeMsg { Pass = pass; Name = name; Version = ver } ->
                actors.Sender    <! AdminJoinMsg { Password = pass; AdminName = name; AdminVersion = ver }
                actors.Scheduler <! Scheduler.AddJob (actors.Receiver, "receive", TimeSpan.FromMilliseconds(10.0))
                return! connecting actors state
            | _ -> return UnhandledMessage
        }
        
        
    idle actors State.empty
