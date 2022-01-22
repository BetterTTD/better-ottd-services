namespace OpenTTD.API


open System
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open OpenTTD.API.Repositories
open OpenTTD.AdminClient
open OpenTTD.AdminClient.Models.Configurations


type IAdminClientProvider =
    abstract member AddClient : tag : string * cfg : ServerConfiguration -> Task<Result<Guid, string>>
    abstract member RemoveClient : id : Guid -> Task<Result<unit, string>>

type AdminClientProvider(
    loggerFactory : ILoggerFactory,
    manager       : IAdminClientManager,
    repository    : IServerConfigurationRepository) =
    
    let logger = loggerFactory.CreateLogger<AdminClientProvider>()
    
    interface IAdminClientProvider with
        member this.AddClient (tag : string, cfg : ServerConfiguration) = task {
            
            logger.LogInformation $"AddClient with tag: #%s{tag}; cfg: %A{cfg}"

            match! repository.TryAdd (tag, cfg) with
            | Ok id ->
                match manager.AttachClient (tag, cfg) with
                | Ok _ ->      return Ok id
                | Error err -> return  Error err
            | Error err -> return Error err
        }
        
        member this.RemoveClient (id : Guid) = task {
            
            logger.LogInformation $"RemoveClient with id: #%O{id}"

            match! repository.TryRemove id with
            | Ok tag ->
                match manager.RemoveClient tag with
                | Ok _ -> return Ok ()
                | Error err -> return  Error err
            | Error err -> return Error err
        }