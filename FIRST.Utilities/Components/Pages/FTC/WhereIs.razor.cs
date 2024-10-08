using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Models;
using Microsoft.AspNetCore.Components;

namespace FIRST.Utilities.Components.Pages.FTC;

public partial class WhereIs
{
    [Inject] private IFtcTeamDataService TeamDataService { get; set; } = null!;
    [Inject] private IFtcMatchDataService MatchDataService { get; set; } = null!;

    private bool _inPlayoffs;
    private IEnumerable<TeamLocation> _teamLocations = [];

    protected override async Task OnInitializedAsync()
    {
        var locations = TeamDataService
            .GetAll()
            .Select(t => new TeamLocation
            {
                TeamNumber = t.TeamNumber,
                TeamName = t.ShortName ?? "N/A",
                LocationType = TeamLocationType.PIT
            });
        
        var activeMatch = await MatchDataService.GetActiveMatch();
        if (activeMatch is null)
        {
            _teamLocations = locations
                .OrderBy(l => l.LocationType)
                .ThenBy(l => l.TeamNumber);
            return;
        }

        if (activeMatch.Match.TournamentLevel != ScheduleType.QUALIFICATION)
        {
            _teamLocations = locations
                .OrderBy(l => l.LocationType)
                .ThenBy(l => l.TeamNumber);
            _inPlayoffs = true;
            return;
        }
        
        var previousMatch = await MatchDataService.GetPreviousQualificationMatch(activeMatch.Match);
        var nextMatch = await MatchDataService.GetNextQualificationMatch(activeMatch.Match);
        var futureMatch = nextMatch is null ? null : await MatchDataService.GetNextQualificationMatch(nextMatch);
        
        var nextMatchTeams =  nextMatch is null 
            ? [] 
            : nextMatch.FtcMatchParticipants
                .Select(p => p.FtcTeam.TeamNumber);
        var futureMatchTeams = futureMatch is null 
            ? []
            : futureMatch.FtcMatchParticipants
                .Select(p => p.FtcTeam.TeamNumber);
        
        var queueingTeams = nextMatchTeams
            .Concat(futureMatchTeams)
            .ToHashSet();
            
        var previousTeams = (previousMatch is null 
            ? []
            : previousMatch.FtcMatchParticipants
                .Select(p => p.FtcTeam.TeamNumber)).ToHashSet();
        
        var activeTeams = activeMatch.Match.FtcMatchParticipants
            .Select(p => p.FtcTeam.TeamNumber)
            .ToHashSet();
        
        locations = locations.Select(l =>
        {
            if (activeTeams.Contains(l.TeamNumber)) l.LocationType = TeamLocationType.FIELD;
            else if (queueingTeams.Contains(l.TeamNumber)) l.LocationType = TeamLocationType.QUEUE;
            else if (previousTeams.Contains(l.TeamNumber)) l.LocationType = TeamLocationType.ENROUTE;
            return l;
        });
        
        _teamLocations = locations
            .OrderBy(l => l.LocationType)
            .ThenBy(l => l.TeamNumber);
    }
}