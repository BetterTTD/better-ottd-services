namespace OpenTTD.API.Storage


open System
open System.Net
open System.Threading.Tasks


type ClientCfg =
    { Id   : Guid
      Tag  : string
      Host : IPAddress
      Port : int
      Pass : string
      Name : string
      Ver  : string }


type IClientReadStorage =
    abstract member All          : Unit                -> Task<ClientCfg []>
    abstract member FindClient   : Guid                -> Task<ClientCfg option>
    abstract member ClientExists : (ClientCfg -> bool) -> Task<bool>

type IClientWriteStorage =
    abstract member Attach       : ClientCfg           -> Task<Result<Guid, string>>
    abstract member Delete       : Guid                -> Task<Result<Guid, string>>