module OpenTTD.AdminClient.Actors.Scheduler


open System
open System.Timers

open Akka.Actor
open Akka.Event
open Akka.FSharp


type JobMessage = obj

type Message =
    | AddJob of IActorRef * JobMessage * TimeSpan
    | PauseJob
    | ResumeJob
    
let init (mailbox : Actor<Message>) =
    let timer = new Timer()
    timer.Start()
    mailbox.Defer (fun _ -> timer.Dispose())
    
    let rec running () =
        actor {
            match! mailbox.Receive () with
            | PauseJob ->
                timer.Stop ()
                return! paused ()
            | _ -> return UnhandledMessage 
        }
        
    and paused () =
        actor {
            match! mailbox.Receive () with
            | ResumeJob ->
                timer.Start ()
                return! running ()
            | _ -> return UnhandledMessage 
        }
        
    and idle () =
        actor {
            match! mailbox.Receive () with
            | AddJob (actor, msg, time) ->
                timer.Interval <- time.TotalMilliseconds
                timer.Elapsed.Add (fun _ -> actor <! msg) 
                return! running ()
            | _ -> return UnhandledMessage 
        }
        
    idle ()