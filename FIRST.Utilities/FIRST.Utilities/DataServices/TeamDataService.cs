using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class TeamDataService(ITeamRepository teams) : ITeamDataService
{
    public async Task<bool> UpsertTeam(Team team)
    {
        var existingTeam = teams.Get(team.ProgramCode, team.TeamNumber, team.SeasonYear);
        if (existingTeam is null) return await teams.Create(team);
        return await teams.Update(existingTeam);
    }

    public bool AreTeamsPresent(string programCode, int seasonYear)
    {
        return teams.Filter(programCode, seasonYear).Any();
    }
}