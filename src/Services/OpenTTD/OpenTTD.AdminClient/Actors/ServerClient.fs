module OpenTTD.AdminClient.Actors.ServerClient


open System
open System.Net.Sockets

open Akka.Event
open Akka.Actor
open Akka.FSharp

open Microsoft.Extensions.Logging
open OpenTTD.AdminClient
open OpenTTD.AdminClient.Models
open OpenTTD.AdminClient.Models.ActorModels
open OpenTTD.AdminClient.Models.Configurations
open OpenTTD.AdminClient.Networking.PacketTransformer
open OpenTTD.AdminClient.Networking.MessageTransformer


type private Actors =
    { Sender    : IActorRef
      Receiver  : IActorRef
      Scheduler : IActorRef }

let init (loggerFactory : ILoggerFactory) (cfg : ServerConfiguration) (mailbox : Actor<Message>) =

    let logger = loggerFactory.CreateLogger "ServerClient"
    logger.LogInformation $"Initializing with configuration: %A{cfg}"
    
    let tcpClient =
        let tcpClient = new TcpClient()
        tcpClient.Connect (cfg.Host, cfg.Port)
        tcpClient
        
    let actors =
        {  Sender    = Sender.init    loggerFactory tcpClient |> spawn mailbox "sender"
           Receiver  = Receiver.init  loggerFactory tcpClient |> spawn mailbox "receiver"
           Scheduler = Scheduler.init loggerFactory           |> spawn mailbox "scheduler" }
    
    mailbox.Defer (fun _ ->
        logger.LogInformation $"Stopping with configuration: %A{cfg}"
        actors.Scheduler <! PoisonPill.Instance
        actors.Sender    <! PoisonPill.Instance
        actors.Receiver  <! PoisonPill.Instance
        tcpClient.Dispose ())

    
    let rec errored actors state =
        actor {
                    
            logger.LogInformation "State: errored"
            
            actors.Scheduler <! Scheduler.PauseJob
            return! errored actors state
        }

    and connected actors state =
        actor {
            
            logger.LogInformation "State: connected"

            match! mailbox.Receive () with
            | PacketReceivedMsg msg ->
                let state = State.dispatch state msg
                
                match msg with
                | ServerChatMsg chatMsg -> ChatCommandHandler.handle (chatMsg, state, actors.Sender)
                | _ -> ()
                
                return! connected actors state
                
            | _ -> return UnhandledMessage
        }
        
    and connecting actors state =
        actor {
            
            logger.LogInformation "State: connecting"

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
            
            logger.LogInformation "State: idle"

            match! mailbox.Receive () with
            | AuthorizeMsg { Pass = pass; Name = name; Version = ver } ->
                actors.Sender    <! AdminJoinMsg { Password = pass; AdminName = name; AdminVersion = ver }
                actors.Scheduler <! Scheduler.AddJob (actors.Receiver, "receive", TimeSpan.FromMilliseconds(10.0))
                return! connecting actors state
            | _ -> return UnhandledMessage
        }
        
        
    idle actors State.empty
