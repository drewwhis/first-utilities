using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.WebServices.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FIRST.Utilities.Components.Pages.FTC;

public partial class LoadEvent : ComponentBase
{
    private bool _isFetching;
    private string? _selectedEventCode;
    private FtcEvent? _activeEvent;
    private bool _teamsLoaded;
    private bool _qualificationMatchesLoaded;
    private bool _playoffMatchesLoaded;
    private bool _yearSet;
    private bool _hasActiveMatch;
    private bool _playoffsActive;
    private IEnumerable<FtcEvent> _events = [];
    
    [Inject] private IFtcApiService FtcApiService { get; set; } = null!;
    [Inject] private IFtcEventDataService FtcEventDataService { get; set; } = null!;
    [Inject] private IActiveProgramSeasonDataService ActiveProgramSeasonDataService { get; set; } = null!;
    [Inject] private IFtcTeamDataService FtcTeamDataService { get; set; } = null!;
    [Inject] private IFtcMatchDataService FtcMatchDataService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        var ftcRecord = ActiveProgramSeasonDataService.GetActiveSeason("FTC");
        if (ftcRecord is null)
        {
            _events = [];
            _activeEvent = null;
            _selectedEventCode = null;
            _teamsLoaded = false;
            _qualificationMatchesLoaded = false;
            _playoffMatchesLoaded = false;
            _yearSet = false;
            _hasActiveMatch = false;
            _playoffsActive = false;
            return;
        }
        
        _yearSet = true;
        _events = FtcEventDataService
            .GetEvents(ftcRecord.SeasonYear)
            .OrderBy(e => e.EventName);
        _activeEvent = FtcEventDataService.GetActiveEvent();
        _selectedEventCode = _activeEvent?.EventCode;

        if (_activeEvent is null) return;
        _teamsLoaded = FtcTeamDataService.AreTeamsPresent();
        _qualificationMatchesLoaded = FtcMatchDataService.AreQualificationMatchesPresent();
        _playoffMatchesLoaded = FtcMatchDataService.ArePlayoffMatchesPresent();

        var activeMatch = await FtcMatchDataService.GetActiveMatch();
        _hasActiveMatch = activeMatch is not null;
        _playoffsActive = _hasActiveMatch &&
                          activeMatch!.Match.TournamentLevel is ScheduleType.SEMIFINAL or ScheduleType.FINAL;
    }

    private async Task FetchEventCodes()
    {
        var ftcRecord = ActiveProgramSeasonDataService.GetActiveSeason("FTC");
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
            await FtcEventDataService.UpsertEvent(new FtcEvent
            {
                EventCode = @event.Code,
                EventName = @event.Name,
                SeasonYear = ftcRecord.SeasonYear
            });
        }
        
        _events = FtcEventDataService
            .GetEvents(ftcRecord.SeasonYear)
            .OrderBy(e => e.EventName);
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }
    
    private async Task ClearLoadedEvents()
    {
        var ftcRecord = ActiveProgramSeasonDataService.GetActiveSeason("FTC");
        if (ftcRecord is null) return;
        
        _isFetching = true;
        await InvokeAsync(StateHasChanged);

        await FtcEventDataService.DeleteEvents(ftcRecord.SeasonYear);
        _events = FtcEventDataService.GetEvents(ftcRecord.SeasonYear);
        _activeEvent = FtcEventDataService.GetActiveEvent();

        await FtcTeamDataService.ClearTeams();
        await FtcMatchDataService.ClearQualificationMatches();
        
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
        
        var ftcRecord = ActiveProgramSeasonDataService.GetActiveSeason("FTC");
        if (ftcRecord is null)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }
        
        await FtcEventDataService.SetActiveEvent(_selectedEventCode, ftcRecord.SeasonYear);
        _activeEvent = FtcEventDataService.GetActiveEvent();
        if (_activeEvent is null)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }

        var teams = await FtcApiService.FetchTeams(_activeEvent.EventCode);
        foreach (var team in teams)
        {
            await FtcTeamDataService.UpsertTeam(new FtcTeam
            {
                FullName = team.NameFull,
                SeasonYear = ftcRecord.SeasonYear,
                ShortName = team.NameShort,
                TeamNumber = team.TeamNumber
            });
        }

        _teamsLoaded = FtcTeamDataService.AreTeamsPresent();
        _qualificationMatchesLoaded = FtcMatchDataService.AreQualificationMatchesPresent();
        _playoffMatchesLoaded = FtcMatchDataService.ArePlayoffMatchesPresent();
        
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task DeselectEvent()
    {
        await FtcEventDataService.ClearActiveEvent();
        _activeEvent = FtcEventDataService.GetActiveEvent();

        await FtcTeamDataService.ClearTeams();
        _teamsLoaded = FtcTeamDataService.AreTeamsPresent();

        await FtcMatchDataService.ClearQualificationMatches();
        _qualificationMatchesLoaded = FtcMatchDataService.AreQualificationMatchesPresent();

        await FtcMatchDataService.ClearPlayoffMatches();
        _playoffMatchesLoaded = FtcMatchDataService.ArePlayoffMatchesPresent();
        
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadQualificationMatches()
    {
        _isFetching = true;
        await InvokeAsync(StateHasChanged);
        
        if (_activeEvent is null || !_teamsLoaded)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }

        var matches = await FtcApiService.FetchMatches(_activeEvent.EventCode, WebServices.Models.FtcApi.ScheduleType.qual);
        foreach (var match in matches)
        {
            var matchTeams = match.Teams?
                .Where(t => t.Station != null)
                .Select(t => new
                {
                    Alliance = t.Station!.StartsWith("Red") ? Alliance.RED : Alliance.BLUE,
                    Station = t.Station!.EndsWith('1') ? 1 : 2,
                    Number = t.TeamNumber
                })
                .ToList() ?? [];
            
            var teamDictionary = matchTeams
                .ToDictionary(
                    t => (t.Alliance, t.Station), 
                    t => t.Number
            );

            var newMatch = new FtcMatch
            {
                MatchNumber = match.MatchNumber,
                Field = match.Field,
                StartTime = match.StartTime,
                Series = match.Series,
                TournamentLevel = ScheduleType.QUALIFICATION
            };
            
            await FtcMatchDataService.CreateMatch(newMatch, teamDictionary);
        }

        _qualificationMatchesLoaded = FtcMatchDataService.AreQualificationMatchesPresent();
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task ClearQualificationMatches()
    {
        await FtcMatchDataService.ClearQualificationMatches();
        _qualificationMatchesLoaded = FtcMatchDataService.AreQualificationMatchesPresent();
        var activeMatch = await FtcMatchDataService.GetActiveMatch();
        _hasActiveMatch = activeMatch is not null;
        _playoffsActive = _hasActiveMatch && activeMatch!.Match.TournamentLevel is ScheduleType.FINAL or ScheduleType.SEMIFINAL;
        await InvokeAsync(StateHasChanged);
    }

    private async Task LoadPlayoffMatches()
    {
        _isFetching = true;
        await InvokeAsync(StateHasChanged);
        
        if (_activeEvent is null || !_teamsLoaded)
        {
            _isFetching = false;
            await InvokeAsync(StateHasChanged);
            return;
        }

        var matches = await FtcApiService.FetchMatches(_activeEvent.EventCode, WebServices.Models.FtcApi.ScheduleType.playoff);
        foreach (var match in matches)
        {
            var matchTeams = match.Teams?
                .Where(t => t.Station != null)
                .Select(t => new
                {
                    Alliance = t.Station!.StartsWith("Red") ? Alliance.RED : Alliance.BLUE,
                    Station = t.Station!.EndsWith('1') ? 1 : 2,
                    Number = t.TeamNumber
                })
                .ToList() ?? [];
            
            var teamDictionary = matchTeams
                .ToDictionary(
                    t => (t.Alliance, t.Station), 
                    t => t.Number
                );

            ScheduleType? tournamentLevel = match.TournamentLevel switch
            {
                "FINAL" => ScheduleType.FINAL,
                "SEMIFINAL" => ScheduleType.SEMIFINAL,
                _ => null
            };

            if (tournamentLevel is null) continue;

            var newMatch = new FtcMatch
            {
                MatchNumber = match.MatchNumber,
                Field = match.Field,
                StartTime = match.StartTime,
                Series = match.Series,
                TournamentLevel = tournamentLevel.Value
            };
            
            await FtcMatchDataService.CreateMatch(newMatch, teamDictionary);
        }

        _playoffMatchesLoaded = FtcMatchDataService.ArePlayoffMatchesPresent();
        _isFetching = false;
        await InvokeAsync(StateHasChanged);
    }

    private async Task ClearPlayoffMatches()
    {
        await FtcMatchDataService.ClearPlayoffMatches();
        _playoffMatchesLoaded = FtcMatchDataService.ArePlayoffMatchesPresent();

        var activeMatch = await FtcMatchDataService.GetActiveMatch();
        _hasActiveMatch = activeMatch is not null;
        _playoffsActive = _hasActiveMatch && activeMatch!.Match.TournamentLevel is ScheduleType.FINAL or ScheduleType.SEMIFINAL;
        await InvokeAsync(StateHasChanged);
    }

    private async Task AdvanceQualificationMatch()
    {
        var activeMatch = await FtcMatchDataService.GetActiveMatch();
        if (activeMatch is null)
        {
            var firstQualificationMatch = FtcMatchDataService
                .GetByScheduleType(ScheduleType.QUALIFICATION)
                .FirstOrDefault(m => m.MatchNumber == 1);
            
            if (firstQualificationMatch is null)
            {
                _hasActiveMatch = false;
                _playoffsActive = false;
                return;
            }
            
            _hasActiveMatch = await FtcMatchDataService.SetActiveMatch(firstQualificationMatch.FtcMatchId);
            _playoffsActive = false;
            return;
        }
        
        var nextMatch = await FtcMatchDataService.GetNextQualificationMatch(activeMatch.Match);
        if (nextMatch is null)
        {
            _hasActiveMatch = true;

            var playoff = FtcMatchDataService
                .GetByScheduleType(ScheduleType.SEMIFINAL)
                .FirstOrDefault(m => m is { MatchNumber: 1, Series: 1 });

            if (playoff is not null)
            {
                await FtcMatchDataService.SetActiveMatch(playoff.FtcMatchId);
            }
            
            _playoffsActive = playoff is not null;
            return;
        }
        
        _hasActiveMatch = await FtcMatchDataService.SetActiveMatch(nextMatch.FtcMatchId);
        _playoffsActive = false;
    }
}