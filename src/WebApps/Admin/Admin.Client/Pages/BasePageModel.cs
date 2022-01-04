using Microsoft.AspNetCore.Components;

namespace Admin.Client.Pages;

public enum PageState
{
    Empty,
    Data,
    Error
}

public abstract class BasePageModel<T> : ComponentBase
{
    public PageState State { get; private set; }
    public T? Model { get; private set; }
    public string? Error { get; private set; }
    public bool IsLoading { get; private set; }

    protected BasePageModel()
    {
        State = PageState.Empty;
    }

    protected abstract Task<T?> InitializeAsync();

    protected async Task Reload() => await InitializeCoreAsync();

    protected void SetAsEmpty(bool isLoading, T? model = default)
    {
        Model = model;
        State = PageState.Empty;
        IsLoading = isLoading;
    }

    protected void SetAsError(string? error)
    {
        Error = error;
        State = PageState.Error;
        IsLoading = false;
    }

    protected void SetAsData(T model)
    {
        if (model is null) throw new ArgumentNullException(nameof(model));
        Model = model;
        State = PageState.Data;
        IsLoading = false;
    }
    
    protected override async Task OnInitializedAsync()
    {
        await InitializeCoreAsync();
        await base.OnInitializedAsync();
    }
    
    private async Task InitializeCoreAsync()
    {
        SetAsEmpty(true);
        
        try
        {
            var data = await InitializeAsync();
            if (data is not null)
                SetAsData(data);
            else
                SetAsEmpty(false);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            SetAsError(e.Message);
        }
    }
}