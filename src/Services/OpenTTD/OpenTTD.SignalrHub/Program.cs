using OpenTTD.SignalrHub.Hubs;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var webApp = builder.Build();
ConfigureApplication(webApp);

webApp.Run();

void ConfigureServices(IServiceCollection services) =>
    services
        .AddLogging()
        .AddCors()
        .AddSignalR();
    
void ConfigureApplication(WebApplication app)
{
    app.UseCors(policyBuilder => policyBuilder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
    app.MapHub<ServerHub>("/server");
    app.MapHub<ChatHub>("/chat");
}