namespace OpenTTD.API.Domain

open System
open System.Net


type DomainError =
    | ClientAlreadyExists of tag : string * host : IPAddress * port : int
    | ClientNotFound      of clientId : Guid
    | Empty

type AppError =
    | Domain of DomainError
    static member create (e : DomainError) = e |> Domain
    static member createResult (e : DomainError) = e |> Domain |> Error