using System.Net.Http.Json;
using Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Admin.Client.Pages;

public class FetchDataPageModel : BasePageModel<WeatherForecastModel[]>
{
    [Inject]
    private HttpClient Http { get; init; } = null!;

    protected override async Task<WeatherForecastModel[]?> InitializeAsync()
    {
        return await Http.GetFromJsonAsync<WeatherForecastModel[]>("sample-data/weather.json");
    }
}