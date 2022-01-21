open Saturn
open Giraffe
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder

let endpointPipe = pipeline {
    plug fetchSession
    plug head
    plug requestId
}

let apiRouter = router {
    get "/" (text "Hello World from Saturn")
}

let topRouter = router {
    not_found_handler SiteMap.page
    forward "/api" apiRouter
}

let configureServices services =
    services

let configureApplication app =
    let env = Environment.getWebHostEnvironment app
    if (env.IsDevelopment()) then
        app.UseDeveloperExceptionPage()
    else
        app

let app = application {
    pipe_through endpointPipe
    
    memory_cache
    
    use_router topRouter
    use_gzip
    
    service_config configureServices
    app_config configureApplication
}

run app