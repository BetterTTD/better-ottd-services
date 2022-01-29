namespace OpenTTD.Domain

open System.Net
open Microsoft.FSharp.Core

type ChatMessage = string

type IPAddressOrClientId =
    | IPAddress of IPAddress
    | ClientId of ClientId

type RconCommand =
    | Ban          of IPAddressOrClientId
    | Kick         of IPAddressOrClientId
    | ClientName   of ClientId * ChatMessage
    | Move         of ClientId * CompanyId
    | ResetCompany of CompanyId
    | Say          of ChatMessage
    | SayClient    of ClientId * ChatMessage
    | Pause
    | Unpause
    override this.ToString() =
        match this with
        | Ban ban ->
            match ban with
            | IPAddress ipAddress  ->  $"ban %s{(ipAddress.ToString())}"
            | ClientId clientId    ->  $"ban %d{clientId}"
            
        | Kick kick ->
            match kick with
            | IPAddress ipAddress  ->  $"kick %s{(ipAddress.ToString())}"
            | ClientId clientId    ->  $"kick %d{clientId}"
            
        | ClientName (clientId, name)   -> $"client_name %d{clientId} %s{name}"
                                        
        | Move (clientId, companyId)    -> $"move %d{clientId} %d{companyId}"
                                        
        | ResetCompany companyId        -> $"reset_company %d{companyId}"
                                        
        | Say message                   -> $"say \"%s{message}\""
        
        | SayClient (clientId, message) -> $"say_client %d{clientId} \"%s{message}\""
        
        | Pause                         -> "pause"
                                             
        | Unpause                       -> "unpause"
        
    
    //| Restart
    //| NewGame