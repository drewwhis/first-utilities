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

    public async Task<bool> SetActiveMatch(int matchId)
    {
        var match = context.FtcMatches.FirstOrDefault(m => m.FtcMatchId == matchId);
        if (match is null) return false;
        
        var currentMatch = context.ActiveFtcMatches.FirstOrDefault();
        if (currentMatch is null)
        {
            try
            {
                await context.ActiveFtcMatches.AddAsync(new ActiveFtcMatch
                {
                    FtcMatchId = matchId,
                });
                return await context.SaveChangesAsync() == 1;
            }
            catch
            {
                return false;
            }
        }

        try
        {
            context.ActiveFtcMatches.Remove(currentMatch);
            if (await context.SaveChangesAsync() != 1) return false;

            var newMatch = new ActiveFtcMatch
            {
                FtcMatchId = matchId
            };
            
            await context.ActiveFtcMatches.AddAsync(newMatch);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ActiveFtcMatch?> GetActiveMatch()
    {
        return await context.ActiveFtcMatches
            .Include(m => m.Match)
            .ThenInclude(m => m.FtcMatchParticipants)
            .ThenInclude(p => p.FtcTeam)
            .FirstOrDefaultAsync();
    }

    public async Task<FtcMatch?> GetNextQualificationMatch(FtcMatch match)
    {
        if (match.TournamentLevel != ScheduleType.QUALIFICATION) return null;
        return await context.FtcMatches
            .Include(m => m.FtcMatchParticipants)
            .ThenInclude(p => p.FtcTeam)
            .FirstOrDefaultAsync(m => 
                m.MatchNumber == match.MatchNumber + 1 
                && m.TournamentLevel == ScheduleType.QUALIFICATION
            );
    }

    public async Task<FtcMatch?> GetPreviousQualificationMatch(FtcMatch match)
    {
        if (match.TournamentLevel != ScheduleType.QUALIFICATION) return null;
        return await context.FtcMatches
            .Include(m => m.FtcMatchParticipants)
            .ThenInclude(p => p.FtcTeam)
            .FirstOrDefaultAsync(m => 
                m.MatchNumber == match.MatchNumber - 1 
                && m.TournamentLevel == ScheduleType.QUALIFICATION
            );    
    }
}