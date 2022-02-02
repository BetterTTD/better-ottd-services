module OpenTTD.AdminClient.Models.ChatHandler


module private Handler =
    let info clientId =
        [ SayClient (clientId, "Team Game - Vanilla server")
          SayClient (clientId, "==========================")
          SayClient (clientId, "Social networks:")
          SayClient (clientId, "* Website - http://www.tg-ottd.org")
          SayClient (clientId, "* Discord - https://discord.gg/hW9DEG7")
          SayClient (clientId, "* Telegram - https://t.me/TG_OpenTTD")
          SayClient (clientId, "* YouTube - https://www.youtube.com/c/iTKerry")
          SayClient (clientId, "==========================")
          SayClient (clientId, "Chat commands:")
          SayClient (clientId, "!whatistg")
          SayClient (clientId, "!rules - read rules in storybook (top menu button)")
          SayClient (clientId, "!reset or !resetme - to remove your company")
          SayClient (clientId, "!name or !rename - to change your nickname")
          SayClient (clientId, "!admin - to call an admin if it's urgent") ]

    let whatIsTg clientId =
        [ SayClient (clientId, "Just check it: https://youtu.be/NsJL1E5oKBM") ]
        
    let admin clientId =
        [ SayClient (clientId, "'!admin' command isn't available yet...") ]

    let rules clientId =
        [ SayClient (clientId, "Rules can be found in storybook (top menu button)") ]

    let reset state = function 
        | { Id = id; Company = company }
            when company.Id = 255uy ->
            [ SayClient (id, "You are a spectator.") ]
            
        | { Id = id; Company = company }
            when state.Clients |> List.exists (fun c -> c.Id <> id && c.Company.Id = company.Id) ->
            [ SayClient (id, "Company can't be removed because you are not alone here.") ]
            
        | { Id = id; Company = company } ->
            [ Move (id, 255uy)
              ResetCompany company.Id
              SayClient (id, $"Company '%s{company.Name}' has been removed.") ]

    let rename clientId name =
        [ ClientName (clientId, name) ]


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

let handle (state : ServerState) (clientId : ClientId, message : ChatMessage) =
    match state.Clients |> List.tryFind (fun client -> client.Id = clientId) with
    | Some client ->
        match message with
        | Info        -> Handler.info     clientId      |> Some 
        | WhatIsTG    -> Handler.whatIsTg clientId      |> Some
        | Admin       -> Handler.admin    clientId      |> Some
        | Rules       -> Handler.rules    clientId      |> Some
        | Reset       -> Handler.reset    state client  |> Some
        | Rename name -> Handler.rename   clientId name |> Some 
        | Unknown     -> None
    | None -> None