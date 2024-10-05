using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class FtcMatchRepository(ApplicationDbContext context) : IFtcMatchRepository
{
    public IEnumerable<FtcMatch> GetAll()
    {
        return context.FtcMatches
            .ToList();
    }

    public FtcMatch? Get(int matchNumber, ScheduleType tournamentLevel)
    {
        return context.FtcMatches.FirstOrDefault(m =>
            m.MatchNumber == matchNumber
            && m.TournamentLevel == tournamentLevel);
    }

    public async Task<bool> Update(FtcMatch ftcMatch)
    {
        try
        {
            context.FtcMatches.Update(ftcMatch);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Create(FtcMatch ftcMatch)
    {
        try
        {
            await context.FtcMatches.AddAsync(ftcMatch);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }
}