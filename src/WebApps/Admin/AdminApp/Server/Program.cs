void ConfigureServices(IServiceCollection services, IConfiguration cfg)
{
    services.AddControllersWithViews();
    services.AddRazorPages();
}

void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
    }
    else
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();

    app.UseRouting();
}

void ConfigureRouting(IEndpointRouteBuilder route)
{
    route.MapControllers();
}

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();
ConfigureApplication(app, app.Environment);
ConfigureRouting(app);

await app.RunAsync();
