using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class FtcTeamDataService(IFtcTeamRepository teams) : IFtcTeamDataService
{
    public async Task<bool> UpsertTeam(FtcTeam ftcTeam)
    {
        var existingTeam = teams.Get(ftcTeam.FtcTeamId);
        if (existingTeam is not null)
        {
            return await teams.Update(existingTeam);
        }
        
        return await teams.Create(ftcTeam);
    }

    public bool AreTeamsPresent()
    {
        return GetAll().Any();
    }

    public IEnumerable<FtcTeam> GetAll()
    {
        return teams.GetAll();
    }
}