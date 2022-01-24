module OpenTTD.AdminClient.ChatCommandHandler


open Akka.Actor
open Akka.FSharp

open FSharpx.Collections
open Microsoft.FSharp.Collections
open OpenTTD.AdminClient.Models.State
open OpenTTD.AdminClient.Networking.MessageTransformer
open OpenTTD.AdminClient.Networking.PacketTransformer
open OpenTTD.Domain


let private (|Info|WhatIsTG|Admin|Rules|Reset|Rename|Unknown|) input =
    match input with
    | "!info"     -> Info
    | "!whatistg" -> WhatIsTG
    | "!admin"    -> Admin
    | "!rules"    -> Rules
    | "!reset"    -> Reset
    | "!resetme"  -> Reset
    
    | str when str.StartsWith "!name " ||
               str.StartsWith "!rename " ->
        Rename (str.Replace("!rename ", "").Replace("!name ", ""))
        
    | _ -> Unknown

let private tryGetChatCommands (msg : ServerChatMessage, state : ServerState) =
    match state.Clients |> List.tryFind (fun client -> client.Id = msg.ClientId) with
    | Some client ->
        match msg.Message with
        | Info -> 
            Some [ SayClient (client.Id, "Team Game - Vanilla server")
                   SayClient (client.Id, "==========================")
                   SayClient (client.Id, "Social networks:")
                   SayClient (client.Id, "* Website - http://www.tg-ottd.org")
                   SayClient (client.Id, "* Discord - https://discord.gg/hW9DEG7")
                   SayClient (client.Id, "* Telegram - https://t.me/TG_OpenTTD")
                   SayClient (client.Id, "* YouTube - https://www.youtube.com/c/iTKerry")
                   SayClient (client.Id, "==========================")
                   SayClient (client.Id, "Chat commands:")
                   SayClient (client.Id, "!whatistg")
                   SayClient (client.Id, "!rules - read rules in storybook (top menu button)")
                   SayClient (client.Id, "!reset or !resetme - to remove your company")
                   SayClient (client.Id, "!name or !rename - to change your nickname")
                   SayClient (client.Id, "!admin - to call an admin if it's urgent") ]
            
        | WhatIsTG ->
            Some [ SayClient (client.Id, "Just check it: https://youtu.be/NsJL1E5oKBM") ]
             
        | Admin ->
            Some [ SayClient (client.Id, "'!admin' command isn't available yet...") ]
            
        | Rules ->
            Some [ SayClient (client.Id, "Rules can be found in storybook (top menu button)") ]
            
        | Reset ->
            match client with
            | { Company = company } when company.Id = 255uy ->
                Some [ SayClient (client.Id, "You are a spectator.") ]
                
            | _ when state.Clients
                       |> List.exists (fun c ->
                           c.Id <> client.Id &&
                           c.Company.Id = client.Company.Id) ->
                Some [ SayClient (client.Id, "Company can't be removed because you are not alone here.") ]
                
            | _ ->
                Some [ Move (client.Id, 255uy)
                       ResetCompany client.Company.Id
                       SayClient (client.Id, $"Company '%s{client.Company.Name}' has been removed.") ]
            
        | Rename name ->
            Some [ ClientName (client.Id, name) ]
            
        | Unknown -> None
    | None -> None

let private tryGetWelcomeCommands (msg : ServerClientInfoMessage, state : ServerState) =
    match state.Clients |> List.tryFind (fun client -> client.Id = msg.ClientId) with
    | Some client -> 
            Some [ SayClient (client.Id, $"Hello %s{client.Name}!")
                   SayClient (client.Id, "Welcome to Team Game - Vanilla server")
                   SayClient (client.Id, "To check server information type '!info' in the chat")
                   SayClient (client.Id, "Also check our 'TG Welcome' & 'TG Public' servers.")
                   SayClient (client.Id, "Enjoy your game!") ]
    | None -> None

let private prepareRconCommands (msg : PacketMessage, state : ServerState) =
    match msg with
    | ServerChatMsg chatMessage -> tryGetChatCommands (chatMessage, state)
    | ServerClientInfoMsg clientInfoMessage -> tryGetWelcomeCommands (clientInfoMessage, state)
    | _ -> None

let handle (msg : PacketMessage, state : ServerState , actor : IActorRef) =
    match prepareRconCommands (msg, state) with
    | Some commands ->
        commands
        |> List.map (fun cmd -> AdminRconMsg { Command = cmd.ToString() })
        |> List.iter (fun cmd -> actor <! cmd)
    | None     -> ()
