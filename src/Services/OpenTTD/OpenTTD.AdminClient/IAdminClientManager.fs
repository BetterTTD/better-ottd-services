namespace OpenTTD.AdminClient

open System

open Akka.Actor
open Akka.FSharp
open FSharpx.Collections

open Microsoft.Extensions.Logging
open Microsoft.FSharp.Collections

open OpenTTD.AdminClient.Actors
open OpenTTD.AdminClient.Models.Configurations
open OpenTTD.AdminClient.Models.ActorModels


type IAdminClientManager =
    abstract member AttachClient : tag : string * cfg : ServerConfiguration -> Result<unit, string>
    abstract member RemoveClient : tag : string -> Result<unit, string>


type AdminClientManager(
    loggerFactory : ILoggerFactory) =

    let logger = loggerFactory.CreateLogger<AdminClientManager>()
    
    let mutable actors = Map.empty
    
    let system =
        Configuration.defaultConfig()
        |> System.create "ottd-system"
    
    interface IAdminClientManager with
        member this.AttachClient (tag : string, cfg : ServerConfiguration) =
            logger.LogInformation $"AttachClient with tag: #%s{tag}; cfg: %A{cfg}"
            
            match actors.ContainsKey tag with
            | false -> 
                let ref = spawn system tag <| ServerClient.init (loggerFactory, cfg) 
                ref <! AuthorizeMsg { Name = cfg.Bot.Name; Pass = cfg.Bot.Pass; Version = cfg.Bot.Ver }
                actors <- actors.Add(tag, ref)
                Ok ()
            | true ->
                Error $"Client already exists with tag #%s{tag}"
            
        member this.RemoveClient (tag : string) =
            logger.LogInformation $"RemoveClient with tag: #%s{tag}"
            
            match actors.TryFind tag with
            | Some ref ->
                ref <! PoisonPill.Instance
                actors <- actors.Remove tag
                Ok ()
            | None ->
                Error $"Client was not found for tag #%s{tag}"