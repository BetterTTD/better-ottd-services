module OpenTTD.AdminClient.Models.State


open FSharpx.Collections
open OpenTTD.AdminClient.Networking.Enums
open OpenTTD.AdminClient.Networking.PacketTransformer


type Company =
    { Id               : byte
      Name             : string
      ManagerName      : string
      Color            : Color
      HasPassword      : bool }

type Client =
    { Id               : uint32
      Company          : Company
      Name             : string
      Host             : string }

type ServerInfo =
    { ServerName       : string
      NetworkRevision  : string
      IsDedicated      : bool
      Landscape        : Landscape
      MapWidth         : int
      MapHeight        : int }

type ServerState =
    { Info             : ServerInfo option
      Clients          : Client     list
      Companies        : Company    list }


let private updateCompanyForClients (clients : Client list, company : Company ) =
    match clients |> List.tryFind (fun cli -> cli.Company.Id = company.Id) with
    | Some client -> (clients |> List.filter (fun cli -> cli.Id <> client.Id)) @ [ { client with Company = company } ]
    | None        -> clients

let empty =
    let spectator =
        { Id          = byte 255
          Name        = "Spectator"
          ManagerName = ""
          Color       = Color.END
          HasPassword = false }
    { Info        = None
      Clients     = []
      Companies   = [ spectator ] }

let dispatch (state : ServerState) (msg : PacketMessage) =
    match msg with
    | ServerWelcomeMsg msg ->
        let info =
            { ServerName      = msg.ServerName
              NetworkRevision = msg.NetworkRevision
              IsDedicated     = msg.IsDedicated
              Landscape       = msg.Landscape
              MapWidth        = msg.MapWidth
              MapHeight       = msg.MapHeight }
        { state with Info = Some info} 

    | ServerClientInfoMsg msg ->
        match state.Companies |> List.tryFind (fun cmp -> cmp.Id = msg.CompanyId) with
        | Some company -> 
            let client =
                { Id       = msg.ClientId
                  Company  = company
                  Name     = msg.Name
                  Host     = msg.Address }
            let clients = state.Clients |> List.filter (fun cli -> cli.Id <> client.Id)
            { state with Clients = clients @ [ client ] }
        | None -> state

    | ServerClientUpdateMsg msg ->
        match state.Clients |> List.tryFind (fun cli -> cli.Id = msg.ClientId),
              state.Companies |> List.tryFind (fun cmp -> cmp.Id = msg.CompanyId) with
        | Some client, Some company ->
            let client  = { client with Name = client.Name; Company = company }
            let clients = state.Clients |> List.filter (fun cli -> cli.Id <> client.Id)
            { state with Clients = clients @ [ client ] }
        | _ -> state

    | ServerClientQuitMsg msg ->
        let clients = state.Clients |> List.filter (fun cli -> cli.Id <> msg.ClientId)
        { state with Clients = clients }

    | ServerClientErrorMsg msg ->
        let clients = state.Clients |> List.filter (fun cli -> cli.Id <> msg.ClientId)
        { state with Clients = clients }
    
    | ServerCompanyInfoMsg msg ->
        let company =
            { Id = msg.CompanyId
              Name = msg.CompanyName
              ManagerName = msg.ManagerName
              Color = msg.Color
              HasPassword = msg.HasPassword }
        let companies = state.Companies |> List.filter (fun cmp -> cmp.Id <> company.Id)
        { state with Companies = companies @ [ company ]
                     Clients   = updateCompanyForClients (state.Clients, company) }
    
    | ServerCompanyUpdateMsg msg ->
        let company =
            { Id = msg.CompanyId
              Name = msg.CompanyName
              ManagerName = msg.ManagerName
              Color = msg.Color
              HasPassword = msg.HasPassword }
        let companies = state.Companies |> List.filter (fun cmp -> cmp.Id <> company.Id)
        { state with Companies = companies @ [ company ]
                     Clients   = updateCompanyForClients (state.Clients, company) }
        
    | ServerCompanyRemoveMsg msg ->
        let companies = state.Companies |> List.filter (fun cmp -> cmp.Id <> msg.CompanyId)
        { state with Companies = companies }

    | _ -> state
