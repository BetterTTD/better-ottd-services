namespace Admin.Client.Pages;

public class ServersPageModel : BasePageModel<ServersPageModel>
{
    protected override async Task<ServersPageModel?> InitializeAsync()
    {
        return await Task.FromResult(new ServersPageModel());
    }
}