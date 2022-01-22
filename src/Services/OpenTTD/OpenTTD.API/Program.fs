open System
open Microsoft.FSharp.Core
open Saturn
open Giraffe

open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging

[<CLIMutable>]
type ClientCreateRequest =
    { Ip      : string
      Port    : int
      Pass    : string
      BotName : string 
      BotVer  : string }
    
[<CLIMutable>]
type ClientUpdateRequest =
    { Ip      : string
      Port    : int
      Pass    : string
      BotName : string 
      BotVer  : string }
    
let getClients : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            return! json "" next ctx
        }

let getClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            return! json id next ctx
        }

let createClient : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let! request = ctx.BindJsonAsync<ClientCreateRequest>()
            return! json request next ctx
        }

let updateClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            let! request = ctx.BindJsonAsync<ClientUpdateRequest>()
            return! json request next ctx
        }

let deleteClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        task {
            return! json id next ctx
        }

let apiRouter = router {
    get "/clients" getClients
    getf "/clients/%O" getClient
    put "/clients/create" createClient
    putf "/clients/%O/update" updateClient
    deletef "/clients/%O/delete" deleteClient
}

let topRouter = router {
    not_found_handler SiteMap.page
    forward "/api" apiRouter
}

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) = l.Equals LogLevel.Debug
    builder.AddFilter(filter)
           .AddConsole()
           .AddDebug()
    |> ignore

let configureServices services =
    services

let configureApplication app =
    let env = Environment.getWebHostEnvironment app
    if (env.IsDevelopment()) then
        app.UseDeveloperExceptionPage()
    else
        app

let app = application {
    use_router topRouter
    logging configureLogging
    service_config configureServices
    app_config configureApplication
}

run app