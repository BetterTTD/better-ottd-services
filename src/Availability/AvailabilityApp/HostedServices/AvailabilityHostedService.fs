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
        try
            let prevState = ottdService.Servers
            
            let online =
                ottdService.Servers
                |> List.filter (fun mem ->
                    ottdProvider.GetLiveServers
                    |> List.map (fun onl -> onl.Name)
                    |> List.distinct
                    |> List.exists ((=) mem.Name))
                |> List.map (fun serv -> { serv with Online = true })
            
            let missing =
                ottdService.Servers
                |> List.filter (fun mem -> online |> List.exists ((<>) mem))
                |> List.map (fun serv -> { serv with Online = false })
            
            missing @ online
            |> List.iter (fun serv -> ottdService.UpdateServer(serv.Name, serv.Online))
            
            let postState = ottdService.Servers
            
            let diff = postState |> List.except prevState
            
            logger.LogInformation $"Servers: %A{ottdService.Servers}"
        with
        | exn -> logger.LogError(exn, "Error!")
    
    member val Timer : Option<Timer> = Option.None with get, set
    
    interface IDisposable with
        member this.Dispose() =
            match this.Timer with
            | Some timer -> timer.Dispose()
            | _          -> ()
            
        
    interface IHostedService with
        member this.StartAsync _ = task {
            ottdService.WatchServer (ServerName "TG Vanilla (reddit legacy)")
            ottdService.WatchServer (ServerName "TG Welcome")
            ottdService.WatchServer (ServerName "TG Public")
            
            this.Timer <- Some (new Timer(job, null, TimeSpan.Zero, TimeSpan.FromSeconds(10)))
            do! Task.CompletedTask
        }
            
        member this.StopAsync _ =
            Task.CompletedTask