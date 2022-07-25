namespace AvailabilityApp

open Configurations
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.FSharp.Control
open Telegram.Bot

type TelegramHostedService(
    client : TelegramBotClient,
    logger : ILogger<TelegramHostedService>,
    configuration : BotConfiguration) =
    
    interface IHostedService with
        member this.StartAsync(ct) = task {
            do! client.DeleteWebhookAsync(true, ct)
            logger.LogInformation($"Setting webhook with %s{configuration.WebhookUrl} %s{configuration.BotToken}")
            
            do! client.SetWebhookAsync($"%s{configuration.WebhookUrl}/%s{configuration.BotToken}")
            logger.LogInformation("End setting webhook")
        }
        
        member this.StopAsync(ct) = task {
            do! client.DeleteWebhookAsync(true, ct)
        }