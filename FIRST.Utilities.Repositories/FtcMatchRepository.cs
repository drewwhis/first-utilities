using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Repositories;

public class FtcMatchRepository(ApplicationDbContext context) : IFtcMatchRepository
{
    public IEnumerable<FtcMatch> GetAll()
    {
        return context.FtcMatches.ToList();
    }

    public IEnumerable<FtcMatch> GetByScheduleType(ScheduleType scheduleType, bool loadTeams)
    {
        return context.FtcMatches
            .Where(m => m.TournamentLevel == scheduleType)
            .Include(m => m.FtcMatchParticipants)
            .ThenInclude(p => p.FtcTeam)
            .ToList();
    }

    public FtcMatch? Get(int matchNumber, int series, ScheduleType tournamentLevel)
    {
        return context.FtcMatches.FirstOrDefault(m =>
            m.MatchNumber == matchNumber
            && m.Series == series
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

    public async Task<FtcMatch?> Create(FtcMatch ftcMatch)
    {
        try
        {
            await context.FtcMatches.AddAsync(ftcMatch);
            if (await context.SaveChangesAsync() != 1) return null;
            await context.Entry(ftcMatch).ReloadAsync();
            return ftcMatch;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> ClearQualificationMatches()
    {
        var entries = context.FtcMatches
            .Where(m => m.TournamentLevel == ScheduleType.QUALIFICATION)
            .ToList();
        
        if (entries.Count == 0) return true;

        try
        {
            context.FtcMatches.RemoveRange(entries);
            return await context.SaveChangesAsync() == entries.Count;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ClearPlayoffMatches()
    {
        var entries = context.FtcMatches
            .Where(m => m.TournamentLevel == ScheduleType.FINAL || m.TournamentLevel == ScheduleType.SEMIFINAL)
            .ToList();
        
        if (entries.Count == 0) return true;

        try
        {
            context.FtcMatches.RemoveRange(entries);
            return await context.SaveChangesAsync() == entries.Count;
        }
        catch
        {
            return false;
        }    }
}