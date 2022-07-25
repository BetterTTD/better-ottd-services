namespace AvailabilityApp.Services

open Domain

open Microsoft.FSharp.Collections
open Microsoft.FSharp.Core


type IOttdService =
    abstract member Servers           : Server list
    abstract member CheckAvailability : name : ServerName                 -> Option<bool>
    abstract member WatchServer       : name : ServerName                 -> unit
    abstract member ForgetServer      : name : ServerName                 -> unit
    abstract member UpdateServer      : name : ServerName * online : bool -> unit

type OttdService() =
    let mutable servers = []
    
    interface IOttdService with
        member this.Servers = servers
        
        member this.CheckAvailability name =
            servers
            |> List.tryFind (fun server -> server.Name = name)
            |> Option.map (fun server -> server.Online)
        
        member this.ForgetServer name =
            servers <- (servers |> List.filter (fun s -> s.Name <> name))
        
        member this.WatchServer name =
            match servers |> List.exists (fun server -> server.Name = name) with 
            | false -> servers <- servers @ [{ Name = name; Online = false }] 
            | true  -> ()
        
        member this.UpdateServer (name, online) =
            let server =
                servers
                |> List.tryFind (fun server -> server.Name = name)
                |> Option.defaultValue { Name = name; Online = online }
            servers <- (servers |> List.filter (fun s -> s.Name <> name)) @
                       [ { server with Online = online } ] 
        