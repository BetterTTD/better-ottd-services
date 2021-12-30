using System.Net.Http.Json;
using Admin.Shared;
using Microsoft.AspNetCore.Components;

namespace Admin.Client.Pages;

public class FetchDataViewModel : BaseViewModel<WeatherForecast[]>
{
    [Inject]
    private HttpClient Http { get; init; } = null!;

    protected override async Task<WeatherForecast[]?> InitializeAsync()
    {
        return await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
    }
}