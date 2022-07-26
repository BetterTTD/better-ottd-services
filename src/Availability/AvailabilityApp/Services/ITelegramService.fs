namespace AvailabilityApp.Services

open System.Threading.Tasks
open Telegram.Bot

type ITelegramService =
    abstract member AddChat         : chatId : int64 -> unit
    abstract member SetDashboardMsg : chatId : int64 * msgId : int64 -> unit
    abstract member SendAlert       : chatId : int64 -> Task<unit>
    abstract member SendDashboard   : chatId : int64 -> Task<unit>
    
type TelegramService(client : TelegramBotClient) =
    let mutable observers = []
    
    let updateObservers (chatId, msgId) =
        let obs = observers |> List.filter (fun (chatId, _) -> chatId <> chatId)
        observers <- obs @ [(chatId, msgId)]
    
    interface ITelegramService with
        member this.AddChat(chatId) =
            updateObservers(chatId, None)
        
        member this.SetDashboardMsg(chatId, msgId) = 
            updateObservers(chatId, Some msgId)

        member this.SendAlert(chatId) = task {
            failwith "todo"
        }
            
        member this.SendDashboard(chatId) = task {
            failwith "todo"
        }