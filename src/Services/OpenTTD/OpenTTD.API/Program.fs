open Saturn

open Microsoft.FSharp.Core
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection

open OpenTTD.AdminClient
open OpenTTD.API
open OpenTTD.API.Repositories

let topRouter = router {
    not_found_handler SiteMap.page
    forward "/api" ApiRouter.routes
}

let configureLogging (builder : ILoggingBuilder) =
    let filter (l : LogLevel) =
        l.Equals LogLevel.Critical ||
        l.Equals LogLevel.Debug ||
        l.Equals LogLevel.Error ||
        l.Equals LogLevel.Information ||
        l.Equals LogLevel.Warning
    builder.AddFilter(filter)
           .AddConsole()
           .AddDebug()
    |> ignore

let configureServices (services : IServiceCollection) =
    services
        .AddSingleton<IAdminClientManager, AdminClientManager>()
        .AddSingleton<IServerConfigurationRepository, InMemoryServerConfigurationRepository>()
        .AddSingleton<IAdminClientProvider, AdminClientProvider>()

let configureApplication (app : IApplicationBuilder) =
    let env = Environment.getWebHostEnvironment app
    if (env.IsDevelopment()) then
        app.UseDeveloperExceptionPage()
    else
        app

let app = application {
    use_router      topRouter
    logging         configureLogging
    service_config  configureServices
    app_config      configureApplication
}

run app