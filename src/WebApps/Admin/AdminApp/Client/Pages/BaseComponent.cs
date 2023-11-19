using Microsoft.AspNetCore.Components;

namespace AdminApp.Client.Pages;

public enum ComponentState
{
    Loading,
    Data,
    NoData
}

public abstract class BaseComponent<TViewModel> : ComponentBase
{
    protected ComponentState State { get; private set; }

    protected TViewModel? ViewModel { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        if (ViewModel is null)
        {
            State = ComponentState.NoData;
        }

        if (ViewModel is not null)
        {
            State = ComponentState.Data;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        State = ComponentState.Loading;
        await InitializeAsync();
    }

    protected abstract Task InitializeAsync();

    protected void RefreshAsync()
    {
        StateHasChanged();   
    }
}