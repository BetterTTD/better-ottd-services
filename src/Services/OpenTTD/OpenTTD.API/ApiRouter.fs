module OpenTTD.API.ApiRouter


open System

open Saturn
open Giraffe

open Microsoft.AspNetCore.Http


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
        return! json "" next ctx
    }

let private updateClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        return! json "" next ctx
    }

let private deleteClient (id : Guid) : HttpHandler =
    fun (next : HttpFunc) (ctx : HttpContext) -> task {
        return! json "" next ctx
    }

let routes = router {
    get "/clients" getClients
    getf "/clients/%O" getClient
    put "/clients/create" createClient
    putf "/clients/%O/update" updateClient
    deletef "/clients/%O/delete" deleteClient
}
