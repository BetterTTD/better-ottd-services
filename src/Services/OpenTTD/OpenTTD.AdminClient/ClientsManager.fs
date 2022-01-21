namespace OpenTTD.AdminClient

open System
open System.Threading.Tasks
open Akka.Actor
open Akka.FSharp

open FSharpx.Collections
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Collections
open Microsoft.FSharp.Control
open OpenTTD.AdminClient.Actors
open OpenTTD.AdminClient.Models.Configurations
open OpenTTD.AdminClient.Models.ActorModels

type PersistedServerConfiguration =
    { Id  : Guid
      Tag : string
      Cfg : ServerConfiguration }

type IServerConfigurationRepository =
    abstract member GetAll     : Task<PersistedServerConfiguration list>
    abstract member TryGetById : Guid -> Task<PersistedServerConfiguration option>
    abstract member TryGet     : (PersistedServerConfiguration -> bool) -> Task<PersistedServerConfiguration option>
    abstract member TryAdd        : tag : string * cfg : ServerConfiguration -> Task<Result<Guid, string>>
    abstract member TryRemove     : Guid -> Task<Result<string, string>>

type InMemoryServerConfigurationRepository() =
    
    let mutable configs = [] 

    interface IServerConfigurationRepository with

        member this.GetAll =
            task {
                return configs
            }
            
        member this.TryGetById id =
            task {
                return configs |> List.tryFind (fun cfg -> cfg.Id = id)
            }
            
        member this.TryGet predicate =
            task {
                return configs |> List.tryFind predicate
            }
            
        member this.TryAdd (tag, cfg) =
            task {
                match configs |> List.exists (fun c ->
                    c.Tag = tag ||
                    c.Cfg.Host = cfg.Host &&
                    c.Cfg.Port = cfg.Port) with
                | true ->
                    return Error $"Such server already exists. tag: %s{tag}; cfg: %A{cfg}"
                    
                | false ->
                    let persistedCfg =
                        { Id = Guid.NewGuid()
                          Tag = tag
                          Cfg = cfg }
                    configs <- configs @ [ persistedCfg ] 
                    return Ok persistedCfg.Id
            }
            
        member this.TryRemove id =
            task {
                match configs |> List.tryFind (fun c ->
                    c.Id = id) with
                | Some cfg ->
                    configs <- configs |> List.filter (fun cfg -> cfg.Id <> cfg.Id)
                    return Ok cfg.Tag
                    
                | None ->
                    return Error $"Server was not found with id: %A{id}"
            }

type ClientsManager(loggerFactory : ILoggerFactory, repository : IServerConfigurationRepository) =

    let logger = loggerFactory.CreateLogger<ClientsManager>()
    
    
    let mutable actors = Map.empty
    
    let system =
        Configuration.defaultConfig()
        |> System.create "ottd-system"
    
    member this.AttachClient (tag : string) (cfg : ServerConfiguration) =
        task {
                
            printfn $"[ClientsManager:AttachClient] tag: %s{tag}; cfg: %A{cfg}"
            
            match! repository.TryGet(fun c -> c.Tag = tag || c.Cfg.Host = cfg.Host && c.Cfg.Port = cfg.Port) with
            | Some _ ->
                return Error $"Client already added for tag #{tag}"
                
            | None ->
                match! repository.TryAdd (tag, cfg) with
                | Ok id -> 
                    let ref = spawn system tag <| ServerClient.init logger cfg 
                    ref <! AuthorizeMsg { Name = cfg.Bot.Name; Pass = cfg.Bot.Pass; Version = cfg.Bot.Ver }
                    actors <- actors.Add(tag, ref)
                    return Ok id
                    
                | Error err ->
                    return Error err
        }
        
    member this.RemoveClient (id : Guid) =
        task {
            
            printfn $"[ClientsManager:RemoveClient] id: {id}"
            
            match! repository.TryRemove id with
            | Ok tag ->
                
                match actors.TryFind tag with
                | Some ref ->
                    ref <! PoisonPill.Instance
                    actors <- actors.Remove tag
                    return Ok ()
                
                | None ->
                    return Error $"Client was not found for tag #{tag}"
                
            | Error error -> return Error error
        }
        
        
        
        