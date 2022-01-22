module OpenTTD.API.ApiRouter


open System
open System.Net

open Saturn
open Giraffe

open OpenTTD.API
open OpenTTD.AdminClient.Models.Configurations

open Microsoft.FSharp.Core
open Microsoft.AspNetCore.Http


type ClientCreateRequest =
    { Ip      : string
      Port    : int
      Pass    : string
      BotName : string 
      BotVer  : string
      Tag     : string }
    
[<CLIMutable>]
type ClientUpdateRequest =
    { Ip      : string
      Port    : int
      Pass    : string
      BotName : string 
      BotVer  : string
      Tag     : string }
    
    
let private getClients : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        return! json "" next ctx
    }

let private getClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        return! json id next ctx
    }

let private createClient : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        let! request = ctx.BindJsonAsync<ClientCreateRequest>()
        let provider = ctx.GetService<IAdminClientProvider>()
        
        let cfg =
            { Host = IPAddress.Parse(request.Ip)
              Port = request.Port
              Bot = { Name = request.BotName
                      Pass = request.Pass
                      Ver = request.BotVer } }
        
        match! provider.AddClient (request.Tag, cfg) with
        | Ok id -> return! json id next ctx
        | Error err -> return! RequestErrors.BAD_REQUEST err next ctx
    }

let private updateClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        let! request = ctx.BindJsonAsync<ClientUpdateRequest>()
        return! json request next ctx
    }

let private deleteClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        let provider = ctx.GetService<IAdminClientProvider>()
        match! provider.RemoveClient id with
        | Ok _ -> return! Successful.OK $"Client with id: {id} removed successfully" next ctx
        | Error err -> return! RequestErrors.BAD_REQUEST err next ctx
    }

let routes = router {
    get "/clients" getClients
    getf "/clients/%O" getClient
    put "/clients/create" createClient
    putf "/clients/%O/update" updateClient
    deletef "/clients/%O/delete" deleteClient
}
