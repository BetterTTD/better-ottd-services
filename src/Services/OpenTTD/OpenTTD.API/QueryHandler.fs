namespace OpenTTD.API


open OpenTTD.API.Domain


type QueryHandler() =
    member this.Handle (query : Query) = 0

