module OpenTTD.AdminClient.Models.Configurations


open System.Net

open OpenTTD.AdminClient.Networking.MessageTransformer
open OpenTTD.AdminClient.Networking.Enums


[<Measure>]
type port

type BotConfiguration =
    { Pass : string
      Name : string
      Ver  : string }

type ServerConfiguration =
    { Host : IPAddress
      Port : int
      Bot  : BotConfiguration }
    

let defaultPolls =
    [ { UpdateType = AdminUpdateType.ADMIN_UPDATE_COMPANY_INFO
        Data       = uint32 0xFFFFFFFF }
      { UpdateType = AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO
        Data       = uint32 0xFFFFFFFF } ]
    |> List.map AdminPollMsg

let defaultUpdateFrequencies =
    [ { UpdateType = AdminUpdateType.ADMIN_UPDATE_CHAT
        Frequency  = AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC }
      { UpdateType = AdminUpdateType.ADMIN_UPDATE_CLIENT_INFO
        Frequency  = AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC }
      { UpdateType = AdminUpdateType.ADMIN_UPDATE_COMPANY_INFO
        Frequency  = AdminUpdateFrequency.ADMIN_FREQUENCY_AUTOMATIC } ]
    |> List.map AdminUpdateFreqMsg
