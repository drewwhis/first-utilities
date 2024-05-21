using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface ITeamDataService
{
    Task<bool> UpsertTeam(Team team);
    bool AreTeamsPresent(string programCode, int seasonYear);
}