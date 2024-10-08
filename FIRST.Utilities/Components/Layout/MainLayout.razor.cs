using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FIRST.Utilities.Components.Layout;

public partial class MainLayout
{
    private bool _isAuthenticated;
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask {get; set;} = null!;
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateTask;
        var user = authState.User;
        _isAuthenticated = user.Identity?.IsAuthenticated ?? false;
    }
}