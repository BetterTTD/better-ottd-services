namespace OpenTTD.API.Domain


open System
open System.Net


type CreateClient =
    { Ip      : string
      Port    : int
      Pass    : string
      BotName : string 
      BotVer  : string
      Tag     : string }

type Command =
    | CreateClient of ip       : IPAddress *
                      port     : int *
                      pass     : string *
                      botName  : string *
                      botVer   : string *
                      tag      : string
                      
    | UpdateClient of clientId : Guid *
                      ip       : IPAddress *
                      port     : int *
                      pass     : string *
                      botName  : string *
                      botVer   : string *
                      tag      : string
                      
    | ConnectClient of clientId : Guid
    | DisconnectClient of clientId : Guid
    | DeleteClient of clientId : Guid