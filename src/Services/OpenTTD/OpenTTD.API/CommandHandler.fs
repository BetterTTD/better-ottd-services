namespace OpenTTD.API


open System
open OpenTTD.API.Domain
open OpenTTD.API.Storage


type CommandHandler(storage : IClientReadStorage) =
    
    let handle (command : Command) = task {
        match command with
        | CreateClient (ipAddress, port, pass, botName, botVer, tag) ->
            match! storage.ClientExists (fun cfg -> cfg.Tag = tag || cfg.Host = ipAddress && cfg.Port = port) with
            | true ->
                return ClientAlreadyExists (tag, ipAddress, port) |> AppError.createResult 
            | false ->
                let cfg =
                    { Id   = Guid.NewGuid()
                      Tag  = tag
                      Host = ipAddress
                      Port = port
                      Pass = pass
                      Name = botName
                      Ver  = botVer }
                return DomainEvent.ClientCreated cfg |> Ok
        
        | UpdateClient (clientId, ipAddress, port, pass, botName, botVer, tag) ->
            match! storage.FindClient clientId with
            | Some client ->
                let cfg =
                    { client with Tag = tag
                                  Host = ipAddress
                                  Port = port
                                  Pass = pass
                                  Name = botName
                                  Ver  = botVer }
                return DomainEvent.ClientUpdated cfg |> Ok
            | None ->
                return DomainError.ClientNotFound clientId |> AppError.createResult
        
        | ConnectClient clientId ->
            match! storage.ClientExists (fun cfg -> cfg.Id = clientId) with
            | true  -> return DomainEvent.ClientConnected clientId |> Ok
            | false -> return DomainError.ClientNotFound  clientId |> AppError.createResult
        
        | DisconnectClient clientId ->
            match! storage.ClientExists (fun cfg -> cfg.Id = clientId) with
            | true  -> return DomainEvent.ClientDisconnected clientId |> Ok
            | false -> return DomainError.ClientNotFound  clientId    |> AppError.createResult
            
        | DeleteClient clientId ->
            match! storage.ClientExists (fun cfg -> cfg.Id = clientId) with
            | true  -> return DomainEvent.ClientDeleted clientId   |> Ok
            | false -> return DomainError.ClientNotFound  clientId |> AppError.createResult
    }
    
    member this.Handle = handle

