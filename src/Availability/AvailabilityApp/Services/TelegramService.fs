namespace AvailabilityApp.Services

open System.Threading.Tasks
open Domain
open Telegram.Bot

type ITelegramService =
    abstract member AddChat         : chatId : int64                  -> unit
    abstract member SetDashboardMsg : chatId : int64 * msgId : int64  -> unit
    abstract member SendAlert       : server : ServerName             -> Task<unit>
    abstract member SendDashboard   : chatId : int64                  -> Task<unit>
    abstract member AddAdmin        : chatId : int64 * login : string -> unit
    abstract member RemoveAdmin     : chatId : int64 * login : string -> unit
    abstract member NotifyDashboards : unit                           -> Task<unit>
    
type TelegramService(client : TelegramBotClient) =
    let mutable observers = []
    let mutable admins    = []
    
    let updateObservers (chatId, msgId) =
        let obs = observers |> List.filter (fun (chatId, _) -> chatId <> chatId)
        observers <- obs @ [(chatId, msgId)]
    
    interface ITelegramService with
        member this.AddChat(chatId) =
            updateObservers(chatId, None)
        
        member this.SetDashboardMsg(chatId, msgId) = 
            updateObservers(chatId, Some msgId)

        member this.SendAlert(server) = task {
            failwith "todo"
        }
            
        member this.SendDashboard(chatId) = task {
            let dashboardText = ""
            let! msg = client.SendTextMessageAsync(chatId, dashboardText)
            updateObservers(chatId, Some msg.MessageId)
        }

        member this.AddAdmin(chatId, login) =
            match admins |> List.tryFind (fun (cId, _) -> cId = chatId) with
            | Some (chatId, logins) ->
                admins <- (admins |> List.filter (fun (cId, logins) -> cId <> chatId)) @
                          [ (chatId, logins @ [ login ]) ]
            | None ->
                admins <- admins @ [ (chatId, [ login ]) ]
        
        member this.RemoveAdmin(chatId, login) =
            match admins |> List.tryFind (fun (cId, _) -> cId = chatId) with
            | Some (chatId, logins) ->
                admins <- (admins |> List.filter (fun (cId, _) -> cId <> chatId)) @
                          [ (chatId, logins |> List.filter ((<>) login) ) ]
            | None -> ()

        member this.NotifyDashboards() = task {
            failwith "todo"
        }