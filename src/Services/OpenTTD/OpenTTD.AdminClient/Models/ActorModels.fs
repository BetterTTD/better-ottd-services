module OpenTTD.AdminClient.Models.ActorModels


open OpenTTD.AdminClient.Networking.PacketTransformer


type Authorize =
    { Name    : string
      Pass    : string
      Version : string }

type Message =
    | PacketReceivedMsg of PacketMessage
    | AuthorizeMsg      of Authorize