using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcTeamDataService
{
    IEnumerable<FtcTeam> GetAll();
    Task<bool> UpsertTeam(FtcTeam ftcTeam);
    bool AreTeamsPresent();
    Task<bool> ClearTeams();
}