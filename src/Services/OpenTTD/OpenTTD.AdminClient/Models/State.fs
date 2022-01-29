module OpenTTD.AdminClient.Models.State


open System.Net
open OpenTTD.AdminClient.Networking.PacketTransformer
open OpenTTD.Domain


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
        Some info |> state.AttachInfo

    | ServerClientInfoMsg msg ->
        let client =
            { Id        = msg.ClientId
              CompanyId = msg.CompanyId
              Name      = msg.Name
              Address   = IPAddress.Parse msg.Address }
        state.CreateClient client

    | ServerClientUpdateMsg msg ->
        let client =
            { Id        = msg.ClientId
              CompanyId = msg.CompanyId
              Name      = msg.Name }
        state.UpdateClient client

    | ServerClientQuitMsg msg ->
        state.RemoveClient msg.ClientId

    | ServerClientErrorMsg msg ->
        state.RemoveClient msg.ClientId
    
    | ServerCompanyInfoMsg msg ->
        let company =
            { Id          = msg.CompanyId
              Name        = msg.CompanyName
              ManagerName = msg.ManagerName
              Color       = msg.Color
              HasPassword = msg.HasPassword }
        state.AttachCompany company
    
    | ServerCompanyUpdateMsg msg ->
        let company =
            { Id          = msg.CompanyId
              Name        = msg.CompanyName
              ManagerName = msg.ManagerName
              Color       = msg.Color
              HasPassword = msg.HasPassword }
        state.AttachCompany company
        
    | ServerCompanyRemoveMsg msg ->
        state.RemoveCompany msg.CompanyId

    | _ -> state
