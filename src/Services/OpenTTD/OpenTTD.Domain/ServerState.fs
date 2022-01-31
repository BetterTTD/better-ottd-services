namespace OpenTTD.Domain

open System.Net
open OpenTTD.Domain

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
        { Id          = byte 255
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