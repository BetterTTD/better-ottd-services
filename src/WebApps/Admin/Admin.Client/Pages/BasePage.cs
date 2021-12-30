using Microsoft.AspNetCore.Components;

namespace Admin.Client.Pages;

public enum PageState
{
    Empty,
    Data,
    Error
}

public abstract class BaseViewModel<T> : ComponentBase
{
    public PageState State { get; private set; }
    public T? Model { get; private set; }
    public string? Error { get; private set; }
    public bool IsLoading { get; protected set; }

    protected BaseViewModel()
    {
        State = PageState.Empty;
    }

    protected abstract Task<T?> InitializeAsync();

    protected async Task Reload() => await InitializeCoreAsync();

    protected override async Task OnInitializedAsync()
    {
        await InitializeCoreAsync();
        await base.OnInitializedAsync();
    }
    
    private async Task InitializeCoreAsync()
    {
        State = PageState.Empty;
        IsLoading = true;
        
        #if DEBUG
        await Task.Delay(TimeSpan.FromSeconds(1));
        #endif

        try
        {
            Model = new Random().Next(1, 5) switch
            {
                1 => await InitializeAsync(),
                2 => default,
                _ => throw new Exception("Test error!"),
            };
            
            State = Model is not null ? PageState.Data : PageState.Empty;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Error = e.Message;
            State = PageState.Error;
        }
        finally
        {
            IsLoading = false;
        }
    }
}