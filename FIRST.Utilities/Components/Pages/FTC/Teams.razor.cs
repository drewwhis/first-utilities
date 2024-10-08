using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using Microsoft.AspNetCore.Components;

namespace FIRST.Utilities.Components.Pages.FTC;

public partial class Teams
{
    [Inject] private IFtcTeamDataService TeamDataService { get; set; } = null!;
    private IEnumerable<FtcTeam> _teams = [];
    
    protected override void OnInitialized()
    {
        _teams = TeamDataService.GetAll().OrderBy(t => t.TeamNumber).ToList();
    }
}