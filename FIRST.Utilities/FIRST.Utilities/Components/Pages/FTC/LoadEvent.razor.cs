using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FIRST.Utilities.Components.Pages.FTC;

public partial class LoadEvent : ComponentBase
{
    private bool _isFetching;
    private string? _selectedEventCode;
    private Event? _activeEvent;
    private bool _teamsLoaded;
    private IEnumerable<Event> _events = [];
    
    [Inject] private IFtcApiService FtcApiService { get; set; } = null!;
    [Inject] private IEventDataService EventDataService { get; set; } = null!;
    [Inject] private IProgramDataService ProgramDataService { get; set; } = null!;
    [Inject] private ITeamDataService TeamDataService { get; set; } = null!;

    protected override void OnInitialized()
    {
        var ftcRecord = ProgramDataService.GetProgram("FTC");
        if (ftcRecord is null)
        {
            _events = [];
            _activeEvent = null;
            _selectedEventCode = null;
            _teamsLoaded = false;
            return;
        }
        
        _events = EventDataService
            .GetEvents(ftcRecord.ProgramCode, ftcRecord.ActiveSeasonYear)
            .OrderBy(e => e.EventName);
        _activeEvent = EventDataService.GetActiveEvent();
        _selectedEventCode = _activeEvent?.EventCode;
        _teamsLoaded = _activeEvent is not null && TeamDataService.AreTeamsPresent(_activeEvent.EventId);
    }

    private async Task FetchEventCodes()
    {
        var ftcRecord = ProgramDataService.GetProgram("FTC");
        if (ftcRecord is null) return;

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
                ProgramCode = ftcRecord.ProgramCode,
                SeasonYear = ftcRecord.ActiveSeasonYear
            });
        }
        
        _events = EventDataService
            .GetEvents(ftcRecord.ProgramCode, ftcRecord.ActiveSeasonYear)
            .OrderBy(e => e.EventName);
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task ClearLoadedEvents()
    {
        var ftcRecord = ProgramDataService.GetProgram("FTC");
        if (ftcRecord is null) return;
        
        _isFetching = true;
        await InvokeAsync(StateHasChanged);

        await EventDataService.DeleteEvents(ftcRecord.ProgramCode, ftcRecord.ActiveSeasonYear);
        _events = EventDataService.GetEvents(ftcRecord.ProgramCode, ftcRecord.ActiveSeasonYear);
        _activeEvent = EventDataService.GetActiveEvent();
        _selectedEventCode = _activeEvent?.EventCode;
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task SaveSelectedEvent()
    {
        _isFetching = true;
        await InvokeAsync(StateHasChanged);

        if (_selectedEventCode is null)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }
        
        var ftcRecord = ProgramDataService.GetProgram("FTC");
        if (ftcRecord is null)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }
        
        await EventDataService.SetActiveEvent(ftcRecord.ProgramCode, _selectedEventCode, ftcRecord.ActiveSeasonYear);
        _activeEvent = EventDataService.GetActiveEvent();
        if (_activeEvent is null)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }

        var teams = await FtcApiService.FetchTeams(_activeEvent.EventCode);
        foreach (var team in teams)
        {
            await TeamDataService.UpsertTeam(new Team
            {
                FullName = team.NameFull,
                ProgramCode = ftcRecord.ProgramCode,
                SeasonYear = ftcRecord.ActiveSeasonYear,
                ShortName = team.NameShort,
                TeamNumber = team.TeamNumber
            }, _activeEvent);
        }

        _teamsLoaded = TeamDataService.AreTeamsPresent(_activeEvent.EventId);
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task DeselectEvent()
    {
        if (_activeEvent is null) return;
        await EventDataService.ClearActiveEvent();
        _activeEvent = EventDataService.GetActiveEvent();
        _teamsLoaded = _activeEvent is not null && TeamDataService.AreTeamsPresent(_activeEvent.EventId);
        await InvokeAsync(StateHasChanged);
    }
}