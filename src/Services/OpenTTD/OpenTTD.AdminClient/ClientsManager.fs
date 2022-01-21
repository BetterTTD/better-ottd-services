namespace OpenTTD.AdminClient

open System
open Akka.Actor
open Akka.FSharp

open OpenTTD.AdminClient.Actors
open OpenTTD.AdminClient.Models.Configurations
open OpenTTD.AdminClient.Models.ActorModels

type ClientsManager() =
    
    let mutable actors = Map.empty
    
    let system =
        Configuration.defaultConfig()
        |> System.create "ottd-system"
    
    member this.AttachClient (tag : string) (cfg : ServerConfiguration) =
        printfn $"[ClientsManager:AttachClient] tag: %s{tag}; cfg: %A{cfg}"
        if actors.ContainsKey(tag) then
            Error $"Client already added for tag #{tag}"
        else
            let ref = spawn system tag <| ServerClient.init cfg 
            ref <! AuthorizeMsg { Name = cfg.Bot.Name; Pass = cfg.Bot.Pass; Version = cfg.Bot.Ver }
            actors <- actors.Add(tag, ref)
            Ok ()
        
    member this.RemoveClient (tag : string) =
        printfn $"[ClientsManager:RemoveClient] tag: %s{tag}"
        match actors.TryFind tag with
        | Some ref ->
            ref <! PoisonPill.Instance
            actors <- actors.Remove tag
            Ok ()
        | None -> Error $"Client was not found for tag #{tag}"