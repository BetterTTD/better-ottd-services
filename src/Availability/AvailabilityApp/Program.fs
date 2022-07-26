module Program

open AvailabilityApp
open AvailabilityApp.Services
open Configurations

open System
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration;
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection

open Giraffe

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

let webApp =
    choose [
        GET >=>
            choose [
                route  "/"           >=> text "index"
                route  "/ping"       >=> text "pong"
                route  "/error"      >=> (fun _ _ -> failwith "Something went wrong!")
            ]
        RequestErrors.notFound (text "Not Found") ]

let configureApp (app : IApplicationBuilder) =
    app.UseGiraffeErrorHandler(errorHandler)
       .UseStaticFiles()
       .UseResponseCaching()
       .UseGiraffe webApp

let configureServices (host : WebHostBuilderContext) (services : IServiceCollection) =
    let cfg = host.Configuration.Get<BotConfiguration>()
    services
        //.AddHostedService<TelegramHostedService>()
        .AddHostedService<AvailabilityHostedService>()
        .AddResponseCaching()
        .AddGiraffe()
        .AddSingleton(cfg)
        //.AddSingleton(TelegramBotClient(cfg.BotToken))
        .AddSingleton<IServersProvider, ServersProvider>()
        .AddSingleton<IOttdService, OttdService>()
        .AddDataProtection()
    |> ignore

let configureLogging (loggerBuilder : ILoggingBuilder) =
    loggerBuilder
        .AddConsole()
        .AddDebug()
    |> ignore

[<EntryPoint>]
let main _ =
    Host.CreateDefaultBuilder()
        .ConfigureWebHostDefaults(
            fun host ->
                host.Configure(configureApp)
                    .ConfigureServices(configureServices)
                    .ConfigureLogging(configureLogging)
                |> ignore)
        .Build()
        .Run()
    0