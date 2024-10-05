using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class FtcTeamRepository(ApplicationDbContext context) : IFtcTeamRepository
{
    public async Task<bool> Create(FtcTeam model)
    {
        try
        {
            await context.FtcTeams.AddAsync(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Update(FtcTeam model)
    {
        try
        {
            context.FtcTeams.Update(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public FtcTeam? Get(int teamId)
    {
        return context.FtcTeams
            .FirstOrDefault(e => e.FtcTeamId == teamId);
    }

    public async Task<bool> ClearTeams()
    {
        var entries = context.FtcTeams.ToList();
        if (entries.Count == 0) return true;

        try
        {
            context.FtcTeams.RemoveRange(entries);
            return await context.SaveChangesAsync() == entries.Count;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<FtcTeam> GetAll()
    {
        return context.FtcTeams.ToList();
    }
}