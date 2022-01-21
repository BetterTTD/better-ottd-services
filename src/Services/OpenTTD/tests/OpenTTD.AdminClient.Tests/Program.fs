module Program

open System.Net
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.FSharp.Core
open OpenTTD.AdminClient
open OpenTTD.AdminClient.Models.Configurations

type TestService(
    cfg: ServerConfiguration,
    manager: ClientsManager) =
    interface IHostedService with
        member this.StartAsync _ =
            match manager.AttachClient "vanilla" cfg with
            | Ok _ -> Task.CompletedTask
            | Error error -> failwith error 
            
        member this.StopAsync _ =
            Task.CompletedTask
        

let configureServices (services : IServiceCollection) =
    let cfg =
        { Host = IPAddress.Parse("127.0.0.1")
          Port = 3977<port>
          Bot = { Name = "TG Bot"
                  Pass = "tg"
                  Ver = "1.0.0" } }
    
    services
        .AddHostedService<TestService>()
        .AddSingleton<ServerConfiguration>(fun sp -> cfg)
        .AddSingleton<ClientsManager>()
    |> ignore

[<EntryPoint>]
let main args =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(configureServices)
        .Build()
        .Run()
    0