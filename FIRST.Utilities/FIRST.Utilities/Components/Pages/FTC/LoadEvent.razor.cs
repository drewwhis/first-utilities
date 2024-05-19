using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Options;
using FIRST.Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace FIRST.Utilities.Components.Pages.FTC;

public partial class LoadEvent : ComponentBase
{
    private bool _isFetching;
    private string? _selectedEventCode;
    private Event? _activeEvent;
    private IEnumerable<Event> _events = [];
    
    [Inject] private IFtcApiService FtcApiService { get; set; } = null!;
    [Inject] private IEventDataService EventDataService { get; set; } = null!;
    [Inject] private IOptions<FtcOptions> FtcOptions { get; set; } = null!;

    protected override void OnInitialized()
    {
        _events = EventDataService
            .GetEvents(FtcOptions.Value.ProgramCode, FtcOptions.Value.CurrentSeason)
            .OrderBy(e => e.EventName);
        _activeEvent = EventDataService.GetActiveEvent();
        _selectedEventCode = _activeEvent?.EventCode;
    }

    private async Task FetchEventCodes()
    {
        _isFetching = true;
        await InvokeAsync(StateHasChanged);
        var fetchedEvents = await FtcApiService.FetchEventList(
            true, 
            true, 
            false
        );
        foreach (var @event in fetchedEvents)
        {
            await EventDataService.UpsertEvent(new Event
            {
                EventCode = @event.Code,
                EventName = @event.Name,
                ProgramCode = FtcOptions.Value.ProgramCode,
                SeasonYear = FtcOptions.Value.CurrentSeason
            });
        }
        
        _events = EventDataService
            .GetEvents(FtcOptions.Value.ProgramCode, FtcOptions.Value.CurrentSeason)
            .OrderBy(e => e.EventName);
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task ClearLoadedEvents()
    {
        _isFetching = true;
        await InvokeAsync(StateHasChanged);

        await EventDataService.DeleteEvents(FtcOptions.Value.ProgramCode, FtcOptions.Value.CurrentSeason);
        _events = EventDataService.GetEvents(FtcOptions.Value.ProgramCode, FtcOptions.Value.CurrentSeason);
        _activeEvent = EventDataService.GetActiveEvent();
        _selectedEventCode = _activeEvent?.EventCode;
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task SaveSelectedEvent()
    {
        if (_selectedEventCode is null) return;
        await EventDataService.SetActiveEvent(FtcOptions.Value.ProgramCode, _selectedEventCode, FtcOptions.Value.CurrentSeason);
        _activeEvent = EventDataService.GetActiveEvent();
        await InvokeAsync(StateHasChanged);
    }
}