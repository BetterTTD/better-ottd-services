using Microsoft.AspNetCore.Components;

namespace AdminApp.Client.Pages;

public enum ComponentState
{
    Loading,
    Data,
    Error
}

public abstract class BaseComponent<TViewModel> : ComponentBase
{
    protected ComponentState State { get; private set; }

    protected TViewModel? ViewModel { get; set; }

    protected string? Error { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        await RefreshAsync();
        
        if (ViewModel is null || Error is not null)
        {
            State = ComponentState.Error;
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

    protected abstract Task RefreshAsync();
}