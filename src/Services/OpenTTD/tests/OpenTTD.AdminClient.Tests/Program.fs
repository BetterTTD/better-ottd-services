module Program

open System.Net
open System.Threading.Tasks
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Core
open OpenTTD.AdminClient
open OpenTTD.AdminClient.Models.Configurations

type TestService(
    cfg: ServerConfiguration,
    manager: ClientsManager) =
    interface IHostedService with
        member this.StartAsync _ =
            task {
                match! manager.AttachClient "vanilla" cfg with
                | Ok id -> printfn $"[TestService:StartAsync] vanilla attached successfully with id: {id}"
                | Error error -> printfn $"[TestService:StartAsync] error: ${error}"
                
                match! manager.AttachClient "vanilla" cfg with
                | Ok _ -> printfn "[TestService:StartAsync] vanilla attached successfully"
                | Error error -> printfn $"[TestService:StartAsync] error: ${error}"
            }
            
        member this.StopAsync _ =
            Task.CompletedTask
        

let configureServices (services : IServiceCollection) =
    let cfg =
        { Host = IPAddress.Parse("")
          Port = 3977<port>
          Bot = { Name = "TG Bot"
                  Pass = ""
                  Ver = "1.0.0" } }
    
    services
        .AddLogging()
        .AddHostedService<TestService>()
        .AddSingleton<ServerConfiguration>(fun sp -> cfg)
        .AddSingleton<ClientsManager>()
        .AddSingleton<IServerConfigurationRepository, InMemoryServerConfigurationRepository>()
    |> ignore
    
let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Debug
    builder.AddFilter(filter)
           .AddConsole()
           .AddDebug()
    |> ignore

[<EntryPoint>]
let main args =
    Host.CreateDefaultBuilder(args)
        .ConfigureServices(configureServices)
        .ConfigureLogging(configureLogging)
        .Build()
        .Run()
    0