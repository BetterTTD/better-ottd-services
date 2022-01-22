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
    
let init (logger : ILogger) (mailbox : Actor<Message>) =
    
    logger.LogInformation "[Scheduler:init]"
    
    let timer = new Timer()
    timer.Start()
    
    mailbox.Defer (fun _ ->
        logger.LogInformation "[Scheduler:stopping] Taking pill instance"
        timer.Dispose())

    let rec running () =
        actor {

            logger.LogInformation "[Scheduler:running]"
            
            match! mailbox.Receive () with
            | PauseJob ->
                timer.Stop ()
                return! paused ()
            | _ -> return UnhandledMessage 
        }
        
    and paused () =
        actor {
            
            logger.LogInformation "[Scheduler:paused]"
            
            match! mailbox.Receive () with
            | ResumeJob ->
                timer.Start ()
                return! running ()
            | _ -> return UnhandledMessage 
        }
        
    and idle () =
        actor {
            
            logger.LogInformation "[Scheduler:idle]"
            
            match! mailbox.Receive () with
            | AddJob (actor, msg, time) ->
                timer.Interval <- time.TotalMilliseconds
                timer.Elapsed.Add (fun _ -> actor <! msg) 
                return! running ()
            | _ -> return UnhandledMessage 
        }
        
    idle ()