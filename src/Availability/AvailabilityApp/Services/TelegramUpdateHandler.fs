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
    
    let (|AddAdmin|_|) (str : string) =
        if str.StartsWith("/add-admin")
        then str.Split "/add-admin"
             |> Seq.filter (fun str -> str <> "/add-admin")
             |> Seq.map (fun str -> str.TrimStart().TrimEnd())
             |> Seq.tryLast
        else None
        
    let (|RemoveAdmin|_|) (str : string) =
        if str.StartsWith("/remove-admin")
        then str.Split "/remove-admin"
             |> Seq.filter (fun str -> str <> "/remove-admin")
             |> Seq.map (fun str -> str.TrimStart().TrimEnd())
             |> Seq.tryLast
        else None
        
open Commands

type TelegramUpdateHandler(ottd : IOttdService, telegram : ITelegramService) =
    let handleMessage (message : Message) = task {
        match message.Text with
        | Start ->
            telegram.AddChat message.Chat.Id
            do! telegram.SendDashboard message.Chat.Id
        
        | AddAdmin name ->
            
            ()
        
        | RemoveAdmin name -> ()
        
        | Watch serverName ->
            ottd.WatchServer serverName
            do! telegram.SendDashboard message.Chat.Id
            
        | Forget serverName ->
            ottd.ForgetServer serverName
            do! telegram.SendDashboard message.Chat.Id
            
        | Display ->
            do! telegram.SendDashboard message.Chat.Id
        
        | _ -> ()
    }
        
    
    member this.Handle(update : Update) = task {
        if update.Type = UpdateType.Message
        then do! handleMessage update.Message
        else ()
    }