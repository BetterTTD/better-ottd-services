module Configurations

[<CLIMutable>]
type BotConfiguration =
    { WebhookUrl : string
      BotToken   : string }