using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using Microsoft.AspNetCore.Components;

namespace FIRST.Utilities.Components.Pages.FTC;

public partial class Matches
{
    [Inject] private IFtcMatchDataService MatchDataService { get; set; } = null!;

    private IEnumerable<FtcMatch> _qualificationMatches = [];
    private IEnumerable<FtcMatch> _playoffMatches = [];

    protected override void OnInitialized()
    {
        _qualificationMatches = MatchDataService.GetByScheduleType(ScheduleType.QUALIFICATION)
            .OrderBy(m => m.MatchNumber);

        var semifinals = MatchDataService.GetByScheduleType(ScheduleType.SEMIFINAL);
        var finals = MatchDataService.GetByScheduleType(ScheduleType.FINAL);

        _playoffMatches = semifinals.Concat(finals)
            .OrderBy(m => m.TournamentLevel)
            .ThenBy(m => m.MatchNumber)
            .ThenBy(m => m.Series);
    }
}