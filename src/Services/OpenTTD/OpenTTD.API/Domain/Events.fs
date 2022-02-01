namespace OpenTTD.API.Domain


open System
open OpenTTD.API.Storage


type DomainEvent =
    | ClientCreated of cfg : ClientCfg
    | ClientUpdated of cfg : ClientCfg
    | ClientConnected of clientId : Guid
    | ClientDisconnected of clientId : Guid
    | ClientDeleted of clientId : Guid

