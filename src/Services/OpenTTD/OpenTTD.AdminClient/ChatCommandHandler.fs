module OpenTTD.AdminClient.ChatCommandHandler

open Akka.Actor
open Akka.FSharp

open Microsoft.FSharp.Collections
open OpenTTD.AdminClient.Models.State
open OpenTTD.AdminClient.Networking.MessageTransformer
open OpenTTD.AdminClient.Networking.PacketTransformer

let private (|Admin|Rules|Reset|Rename|Unknown|) input =
    match input with
    | "!admin"   -> Admin
    | "!rules"   -> Rules
    | "!reset"   -> Reset
    | "!resetMe" -> Reset
    
    | str when str.StartsWith "!name " ||
               str.StartsWith "!rename " ->
        Rename (str.Replace("!rename ", "").Replace("!name ", ""))
        
    | _ ->
        Unknown

let say_client id msg =
    AdminRconMsg { Command = $"say_client %d{id} \"%s{msg}\"" }

let client_name id name =
    AdminRconMsg { Command = $"client_name %d{id} \"%s{name}\"" }

let move idFrom idTo =
    AdminRconMsg { Command = $"move %d{idFrom} %d{idTo}" }
    
let reset_company id =
    AdminRconMsg { Command = $"reset_company %d{id}" }

let prepareRconCmd (msg : ServerChatMessage, state : ServerState) =
    match state.Clients |> List.tryFind (fun client -> client.Id = msg.ClientId) with
    | Some client ->
        match msg.Message with
        | Admin -> Some [ say_client client.Id "'!admin' command isn't available yet..." ]
        | Rules -> Some [ say_client client.Id "There will be some rules. Later..." ]
        | Reset -> Some [ move client.Id 255; reset_company client.Company.Id ]
        | Rename name -> Some [ client_name client.Id name ]
        | Unknown -> None
    | None -> None

let handle (msg : ServerChatMessage, state : ServerState , actor : IActorRef) =
    match prepareRconCmd (msg, state) with
    | Some commands -> commands |> List.iter (fun cmd -> actor <! cmd)
    | None     -> ()
