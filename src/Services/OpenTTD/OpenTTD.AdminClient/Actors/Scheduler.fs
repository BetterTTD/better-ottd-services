module OpenTTD.AdminClient.Actors.Scheduler


open System
open System.Timers

open Akka.Actor
open Akka.Event
open Akka.FSharp
open Microsoft.Extensions.Logging


type JobMessage = obj

type Message =
    | AddJob of IActorRef * JobMessage * TimeSpan
    | PauseJob
    | ResumeJob
    
let init (loggerFactory : ILoggerFactory) (mailbox : Actor<Message>) =
    
    let logger = loggerFactory.CreateLogger "Scheduler"
    logger.LogInformation "Initializing"
    
    let timer = new Timer()
    timer.Start()
    
    mailbox.Defer (fun _ ->
        logger.LogInformation "Stopping"
        timer.Dispose())

    let rec running () =
        actor {

            logger.LogInformation "State: running"
            
            match! mailbox.Receive () with
            | PauseJob ->
                timer.Stop ()
                return! paused ()
            | _ -> return UnhandledMessage 
        }
        
    and paused () =
        actor {
            
            logger.LogInformation "State: paused"
            
            match! mailbox.Receive () with
            | ResumeJob ->
                timer.Start ()
                return! running ()
            | _ -> return UnhandledMessage 
        }
        
    and idle () =
        actor {
            
            logger.LogInformation "State: idle"
            
            match! mailbox.Receive () with
            | AddJob (actor, msg, time) ->
                timer.Interval <- time.TotalMilliseconds
                timer.Elapsed.Add (fun _ -> actor <! msg) 
                return! running ()
            | _ -> return UnhandledMessage 
        }
        
    idle ()