using Microsoft.AspNetCore.Components.Routing;

namespace FIRST.Utilities.Components.Layout;

public partial class NavMenu
{
    private static readonly string DownArrow = "bi-caret-down-fill";
    private static readonly string RightArrow = "bi-caret-right-fill";
    
    private string? currentUrl;

    private bool _expandFtc;
    private bool _expandFllChallenge;

    private void ToggleFtc() => _expandFtc = !_expandFtc;
    private void ToggleFllChallenge() => _expandFllChallenge = !_expandFllChallenge;

    protected override void OnInitialized()
    {
        currentUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}