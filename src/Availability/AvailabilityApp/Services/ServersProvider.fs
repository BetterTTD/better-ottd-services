namespace AvailabilityApp.Services

open Domain

open FSharp.Data
open Microsoft.Extensions.Logging


type IServersProvider =
    abstract member GetLiveServers : Server list

type ServersProvider(logger : ILogger<ServersProvider>) =
    let [<Literal>] ServerListing = "https://servers.openttd.org/listing"
    let serverListing = new HtmlProvider<ServerListing>()

    interface IServersProvider with
        member this.GetLiveServers =
            logger.LogInformation("Fetch servers table from {ServerListing}", ServerListing)
            serverListing.Tables.``Server-table``.Rows
            |> Seq.map (fun x -> { Name = ServerName x.Name; Online = true })
            |> Seq.toList