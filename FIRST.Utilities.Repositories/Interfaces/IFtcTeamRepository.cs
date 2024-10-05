using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IFtcTeamRepository
{
    Task<bool> Create(FtcTeam model);
    Task<bool> Update(FtcTeam model);
    FtcTeam? Get(int teamId);
    Task<bool> ClearTeams();
    IEnumerable<FtcTeam> GetAll();
}