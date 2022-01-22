namespace OpenTTD.Domain

open System.Net
open Microsoft.FSharp.Core

type CompanyId = uint32
type ClientId = uint32

type IPAddressOrClientId =
    | IPAddress of IPAddress
    | ClientId of ClientId

type RconCommand =
    | Ban          of IPAddressOrClientId
    | Kick         of IPAddressOrClientId
    | ClientName   of ClientId * string
    | Move         of ClientId * CompanyId
    | ResetCompany of CompanyId
    | Say          of string
    | SayClient    of ClientId * string
    | Pause
    | Unpause
    override this.ToString() =
        match this with
        | Ban ban ->
            let cmd = sprintf "ban %s"
            match ban with
            | IPAddress ipAddress -> cmd (ipAddress.ToString())
            | ClientId clientId -> cmd (clientId.ToString())
            
        | Kick kick ->
            let cmd = sprintf "kick %s"
            match kick with
            | IPAddress ipAddress -> cmd (ipAddress.ToString())
            | ClientId clientId -> cmd (clientId.ToString())
            
        | ClientName (id, name)         -> $"client_name %d{id} %s{name}"
                                        
        | Move (clientId, companyId)    -> $"move %d{clientId} %d{companyId}"
                                        
        | ResetCompany companyId        -> $"reset_company %d{companyId}"
                                        
        | Say message                   -> $"say \"%s{message}\""
        
        | SayClient (clientId, message) -> $"say_client %d{clientId} \"%s{message}\""
        
        | Pause                         -> "pause"
                                             
        | Unpause                       -> "unpause"
        
    
    //| Restart
    //| NewGame