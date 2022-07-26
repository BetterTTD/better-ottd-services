namespace AvailabilityApp

open Domain

open System
open System.Threading;
open System.Threading.Tasks
open AvailabilityApp.Services
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Collections

type AvailabilityHostedService(
    logger       : ILogger<AvailabilityHostedService>,
    ottdService  : IOttdService,
    ottdProvider : IServersProvider) =
    
    let job _ =
        let prevState = ottdService.Servers
        let allLiveServers = ottdProvider.GetLiveServers
        
        let online =
            ottdService.Servers
            |> List.filter (fun mem ->
                allLiveServers
                |> List.map (fun onl -> onl.Name)
                |> List.distinct
                |> List.exists ((=) mem.Name))
            |> List.map (fun serv -> { serv with Online = true })
        
        let missing =
            ottdService.Servers
            |> List.filter (fun mem -> online |> List.exists ((<>) mem))
            |> List.map (fun serv -> { serv with Online = false })
        
        let diff = missing @ online |> List.except prevState
        diff |> List.iter (fun serv -> ottdService.UpdateServer(serv.Name, serv.Online))
        
        
        
        ()
    
    member val Timer : Option<Timer> = Option.None with get, set
    
    interface IDisposable with
        member this.Dispose() =
            match this.Timer with
            | Some timer -> timer.Dispose()
            | _          -> ()
            
        
    interface IHostedService with
        member this.StartAsync _ = task {
            [ ServerName "TG Vanilla (reddit legacy)"
              ServerName "TG Welcome"
              ServerName "TG Public" ]
            |> List.iter ottdService.WatchServer 
            
            this.Timer <- Some (new Timer(job, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)))
            do! Task.CompletedTask
        }
            
        member this.StopAsync _ = task {
            match this.Timer with
            | Some timer -> timer.Change(Timeout.Infinite, 0) |> ignore
            | _          -> ()
            do! Task.CompletedTask
        }
            