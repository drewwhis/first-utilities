using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcTeamDataService
{
    Task<bool> UpsertTeam(FtcTeam ftcTeam);
    bool AreTeamsPresent();
    Task<bool> ClearTeams();
}