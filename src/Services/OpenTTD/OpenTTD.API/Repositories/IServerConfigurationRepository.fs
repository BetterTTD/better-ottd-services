namespace OpenTTD.API.Repositories

open System
open System.Threading.Tasks

open OpenTTD.AdminClient.Models.Configurations

type PersistedServerConfiguration =
    { Id  : Guid
      Tag : string
      Cfg : ServerConfiguration }

type IServerConfigurationRepository =
    abstract member GetAll     : Task<PersistedServerConfiguration list>
    abstract member TryGetById : Guid -> Task<PersistedServerConfiguration option>
    abstract member TryGet     : (PersistedServerConfiguration -> bool) -> Task<PersistedServerConfiguration option>
    abstract member TryAdd     : tag : string * cfg : ServerConfiguration -> Task<Result<Guid, string>>
    abstract member TryRemove  : Guid -> Task<Result<string, string>>

type InMemoryServerConfigurationRepository() =
    
    let mutable configs = [] 

    interface IServerConfigurationRepository with

        member this.GetAll = task {
            return configs
        }
            
        member this.TryGetById id = task {
            return configs |> List.tryFind (fun cfg -> cfg.Id = id)
        }
            
        member this.TryGet predicate = task {
            return configs |> List.tryFind predicate
        }
            
        member this.TryAdd (tag, cfg) = task {
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
            
        member this.TryRemove id = task {
            match configs |> List.tryFind (fun c ->
                c.Id = id) with
            | Some cfg ->
                configs <- configs |> List.filter (fun cfg -> cfg.Id <> cfg.Id)
                return Ok cfg.Tag
                
            | None ->
                return Error $"Server was not found with id: %A{id}"
        }
