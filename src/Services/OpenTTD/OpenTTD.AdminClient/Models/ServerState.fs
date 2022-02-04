namespace OpenTTD.AdminClient.Models

open System.Net
open OpenTTD.AdminClient.Networking.Enums
open OpenTTD.AdminClient.Networking.PacketTransformer

type CompanyId = byte
type ClientId  = uint32

type CreateClient =
    { Id               : ClientId
      CompanyId        : CompanyId
      Address          : IPAddress
      Name             : string }

type UpdateClient =
    { Id               : ClientId
      CompanyId        : CompanyId
      Name             : string }
    
type Company =
    { Id               : CompanyId
      Name             : string
      ManagerName      : string
      Color            : Color
      HasPassword      : bool }
    static member Spectator =
        { Id          = 255uy
          Name        = "Spectator"
          ManagerName = ""
          Color       = Color.END
          HasPassword = false }

type Client =
    { Id               : ClientId
      Company          : Company
      Name             : string
      Host             : IPAddress }

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
    static member Empty =
        { Info       = None
          Clients    = [ ]
          Companies  = [ Company.Spectator ] }
    
    member this.AttachInfo info =
        { this with Info = info }
        
    member this.CreateClient (create: CreateClient) =
        match this.Companies |> List.tryFind (fun company -> company.Id = create.CompanyId) with
        | Some company ->
            let client =
                { Id       = create.Id
                  Company  = company
                  Name     = create.Name
                  Host     = create.Address }
            let clients =
                this.Clients
                |> List.filter (fun c -> c.Id <> client.Id)
            { this with Clients = clients @ [ client ] }
        | None -> this
        
    member this.UpdateClient (update : UpdateClient) =
        match this.Companies |> List.tryFind (fun company -> company.Id = update.CompanyId),
              this.Clients   |> List.tryFind (fun client  -> client.Id  = update.Id) with
        | Some company, Some oldClient ->
            let updatedClient =
                { oldClient with Name    = update.Name
                                 Company = company }
            let clients =
                this.Clients
                |> List.filter (fun c -> c.Id <> updatedClient.Id)
            { this with Clients = clients @ [ updatedClient ] }
        | _ -> this
        
    member this.AttachCompany (company : Company) =
        let companyClients =
            this.Clients
            |> List.filter (fun cli -> cli.Company.Id = company.Id)
            |> List.map    (fun cli -> { cli with Company = company })
        let otherClients   = 
            this.Clients
            |> List.filter (fun cli -> cli.Company.Id <> company.Id)
        let companies      =
            this.Companies
            |> List.filter (fun com -> com.Id <> company.Id)
        { this with Companies = companies    @ [ company ]
                    Clients   = otherClients @ companyClients }
        
    member this.RemoveCompany companyId =
        let companyClients =
            this.Clients
            |> List.filter (fun cli -> cli.Company.Id = companyId)
            |> List.map    (fun cli -> { cli with Company = Company.Spectator })
        let otherClients   =
            this.Clients
            |> List.filter (fun cli -> cli.Company.Id <> companyId)
        let companies      =
            this.Companies
            |> List.filter (fun x -> x.Id <> companyId)
        { this with Companies = companies
                    Clients   = otherClients @ companyClients }
    
    member this.RemoveClient clientId =
        let clients = this.Clients |> List.filter (fun x -> x.Id <> clientId)
        { this with Clients = clients }
        
    static member Dispatch (state : ServerState) (msg : PacketMessage) =
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
