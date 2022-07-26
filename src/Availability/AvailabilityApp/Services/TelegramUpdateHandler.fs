namespace AvailabilityApp.Services

open AvailabilityApp.Services
open Domain
open Telegram.Bot.Types
open Telegram.Bot.Types.Enums

module Commands =
    let (|Start|_|) (str : string) =
        if str = "/start" then Some Start else None
    
    let (|Watch|_|) (str : string) =
        if str.StartsWith("/watch")
        then str.Split "/watch"
             |> Seq.filter (fun str -> str <> "/watch")
             |> Seq.map (fun str -> str.TrimStart().TrimEnd())
             |> Seq.tryLast
             |> Option.map ServerName
        else None

    let (|Forget|_|) (str : string) =
        if str.StartsWith("/forget")
        then str.Split "/forget"
             |> Seq.filter (fun str -> str <> "/forget")
             |> Seq.map (fun str -> str.TrimStart().TrimEnd())
             |> Seq.tryLast
             |> Option.map ServerName
        else None

    let (|Display|_|) (str : string) =
        if str.StartsWith("/display")
        then Some Display
        else None

open Commands

type TelegramUpdateHandler(ottd : IOttdService, telegram : ITelegramService) =
    let handleMessage (message : Message) = task {
        match message.Text with
        | Start ->
            observers <- observers @ [ (message.Chat.Id, None) ]
            do! display message.Chat.Id
            
        | Watch serverName ->
            ottd.WatchServer serverName
            do! display message.Chat.Id
            
        | Forget serverName ->
            ottd.ForgetServer serverName
            do! display message.Chat.Id
            
        | Display -> do! display message.Chat.Id
            
        | _ -> ()
    }
        
    
    member this.Handle(update : Update) = task {
        if update.Type = UpdateType.Message
        then do! handleMessage update.Message
        else ()
    }