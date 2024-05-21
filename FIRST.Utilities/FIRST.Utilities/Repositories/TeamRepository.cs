using FIRST.Utilities.Data;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class TeamRepository(ApplicationDbContext context) : ITeamRepository
{
    public async Task<bool> Create(Team model)
    {
        try
        {
            await context.Teams.AddAsync(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Update(Team model)
    {
        try
        {
            context.Teams.Update(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public Team? Get(string programCode, int teamNumber, int seasonYear)
    {
        return context.Teams.FirstOrDefault(e =>
            e.ProgramCode == programCode
            && e.TeamNumber == teamNumber
            && e.SeasonYear == seasonYear);
    }

    public IEnumerable<Team> Filter(string programCode, int seasonYear)
    {
        return context.Teams
            .Where(t => t.ProgramCode == programCode && t.SeasonYear == seasonYear)
            .ToList();
    }
}